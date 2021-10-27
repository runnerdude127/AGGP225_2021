using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 1f;
    public int health = 100;
    int currentHealth;
    public int charge = 100;
    int currentCharge;
    float chargeSpeed = 1;
    bool cooldownCharge = false;

    public Color color;
    public Color testColor;
    public Material playerColor;
    // opposite : new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);

    public Transform shootPos;
    public GameObject rayShot;

    public GameObject userTag;
    public TextMesh myName;
    private Meter headHealthBar;

    public Meter healthBar;
    public Meter chargeBar;

    public CharacterController characterController;
    float horizontal;
    float vertical;
    float velocity;

    CameraManagaer cam;
    Vector3 camDir;

    AudioSource source;
    //online
    bool walkcycle = false;
    public AudioClip footstep;
    public AudioClip jump;
    public AudioClip colorChange;
    public AudioClip takeDamage;
    public AudioClip shoot;
    //client
    public AudioClip chargeSound;
    public AudioClip fullyCharged;
    public AudioClip chargeDepleted;
    public AudioClip noCharge;
    public AudioClip hitConfirm;

    public Animator playerAnim;

    private void Awake()
    {
        

        source = gameObject.GetComponent<AudioSource>();
        characterController = gameObject.GetComponent<CharacterController>();
        playerColor = gameObject.GetComponentInChildren<MeshRenderer>().material;
        cam = gameObject.GetComponent<CameraManagaer>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();
        myName = userTag.GetComponentInChildren<TextMesh>();
        headHealthBar = GetComponentInChildren<Meter>();

        currentHealth = health;
        PlayerGUI.instance.thisPlayerHealth.SetMax(health);

        StartCoroutine(ChargeBack());
        currentCharge = charge;
        PlayerGUI.instance.thisPlayerCharge.SetMax(charge);

        if (gameObject.GetPhotonView().IsMine)
        {
            color = PhotonManager.instance.myColor;
            testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;
            gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);
            gameObject.GetPhotonView().RPC("playerTagUpdate", RpcTarget.AllBufferedViaServer, PhotonManager.instance.myUsername);
        }
    }

    private void Update()
    {
        userTag.transform.rotation = Quaternion.LookRotation(userTag.transform.position - Camera.main.transform.position);
        if (gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetPhotonView().RPC("setShoot", RpcTarget.All);
            PlayerGUI.instance.thisPlayerHealth.SetCurrent(currentHealth);
            PlayerGUI.instance.thisPlayerCharge.SetCurrent(currentCharge);          

            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;

            //Debug.Log("grounded: " + characterController.isGrounded + " velocity: " + velocity);

            if (PlayerGUI.instance.playerKeyInput == true)
            {
                horizontal = Input.GetAxis("Horizontal") * speed;
                vertical = Input.GetAxis("Vertical") * speed;
                if (characterController.isGrounded)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        gameObject.GetPhotonView().RPC("playJumpSound", RpcTarget.All);
                        velocity = jumpSpeed;
                    }
                    else
                    {
                        velocity = 0;
                    }
                }
            }
            else
            {
                horizontal = 0;
                vertical = 0;
            }

            AnimationHandler();

            
            velocity += Physics.gravity.y * Time.deltaTime;

            camDir = cam.GetCameraDirection();
            if (cam != null)
            {
                cam.OnStartFollowing();
            }

            characterController.Move((gameObject.transform.right * horizontal + gameObject.transform.forward * vertical + (new Vector3(0, 1, 0) * velocity)) * Time.deltaTime);

            if (PlayerGUI.instance.playerMouseInput == true)
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, camDir.y, gameObject.transform.eulerAngles.z);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!cooldownCharge)
                    {
                        if ((currentCharge - 2) >= 0)
                        {
                            //gameObject.GetPhotonView().RPC("shootWeapon", RpcTarget.All, color.r, color.g, color.b);
                            shootWeapon(color.r, color.g, color.b);
                            PlayerGUI.instance.thisPlayerCharge.ResetMeter(currentCharge);
                            currentCharge = currentCharge - 2;
                            chargeSpeed = 1;
                        }
                        else
                        {
                            //gameObject.GetPhotonView().RPC("shootWeapon", RpcTarget.All, color.r, color.g, color.b);
                            shootWeapon(color.r, color.g, color.b);
                            PlayerGUI.instance.thisPlayerCharge.ResetMeter(currentCharge);
                            currentCharge = 0;
                            cooldownCharge = true;
                            source.PlayOneShot(chargeDepleted);
                        }
                    }
                    else
                    {
                        source.PlayOneShot(noCharge);
                    }

                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    gameObject.GetPhotonView().RPC("playerHurt", RpcTarget.All, 10, testColor.r, testColor.g, testColor.b);
                    //gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, testColor.r, testColor.g, testColor.b);
                    //testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                }
            }

            if (cooldownCharge)
            {
                PlayerGUI.instance.chargeColor.color = new Color(.5f, .5f, .5f);
            }
            else
            {
                PlayerGUI.instance.chargeColor.color = new Color(1, 1, 1);
            }
        }
    }

    IEnumerator ChargeBack()
    {
        yield return new WaitForSeconds(chargeSpeed);
        //source.PlayOneShot(chargeSound, currentCharge);
        currentCharge = currentCharge + 1;
        if (currentCharge < charge)
        {
            if (chargeSpeed <= .02f)
            {
                chargeSpeed = .02f;
            }
            else
            {
                chargeSpeed = chargeSpeed / 1.5f;
            }
            StartCoroutine(ChargeBack());
        }
        else
        {
            if (currentCharge > charge)
            {
                currentCharge = charge;
            }
            cooldownCharge = false;
            source.PlayOneShot(fullyCharged);
            yield return new WaitUntil(() => currentCharge < charge);
            chargeSpeed = 1;
            StartCoroutine(ChargeBack());
        }
    }

    void shootWeapon(float r, float g, float b)
    {
        gameObject.GetPhotonView().RPC("playShootSound", RpcTarget.All); 
        Debug.DrawRay(transform.position, cam.GetCameraFacing() * 1000, Color.red, 10);
        //Debug.Log(cam.GetCameraFacing());
        if (gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetPhotonView().RPC("setShoot", RpcTarget.All); 
        }
        gameObject.GetPhotonView().RPC("makeShot", RpcTarget.All, r, g, b);
        playerAnim.ResetTrigger("shoot");
        playerAnim.SetTrigger("shoot");
    }

    [PunRPC]

    void setShoot()
    {
        if (cam)
        {
            if (cam.cameraTransform)
            {
                shootPos.position = cam.cameraTransform.position - new Vector3(0, 0.1f, 0);
                shootPos.rotation = Quaternion.Euler(camDir);
            }
        }       
    }

    [PunRPC]
    void makeShot(float r, float g, float b)
    {       
        GameObject shotFired = Instantiate(rayShot, shootPos.position, shootPos.rotation);
        RayShot myShot = shotFired.GetComponent<RayShot>();
        myShot.colorSet(r, g, b);
        myShot.parentSet(this.gameObject);
    }

    public void playHitConfirmSound()
    {
        source.PlayOneShot(hitConfirm);
    }

    void AnimationHandler()
    {
        playerAnim.SetBool("grounded", characterController.isGrounded);
        if (horizontal != 0 || vertical != 0)
        {
            playerAnim.SetBool("walking", true);
            if (walkcycle == false)
            {
                StartCoroutine(stepcycle());             
            }
        }
        else
        {
            playerAnim.SetBool("walking", false);
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerAnim.SetBool("jumping", true);
            }
            else
            {
                playerAnim.SetBool("jumping", false);
            }

        }
    }

    [PunRPC]
    void playerTagUpdate(string nameSet)
    {
        headHealthBar = GetComponentInChildren<Meter>();
        headHealthBar.SetMax(health);
        headHealthBar.SetMainColor(playerColor.color);
        this.myName.text = nameSet;
    }

    [PunRPC]
    void updateHealth(int ch, float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        headHealthBar.SetCurrent(ch);
        headHealthBar.SetMainColor(playerColor.color);
        headHealthBar.SetLossColor(c);
    }

    [PunRPC]
    public void playerHurt(int damage, float r, float g, float b)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            Color oldColor = new Color(PlayerGUI.instance.infectColor.color.r, PlayerGUI.instance.infectColor.color.g, PlayerGUI.instance.infectColor.color.b);
            Color colorInflict = new Color(r, g, b);
            playerAnim.ResetTrigger("damage");
            playerAnim.SetTrigger("damage");
            
            if (playerColor.color != colorInflict)
            {
                gameObject.GetPhotonView().RPC("playDamageSound", RpcTarget.All);
                currentHealth = currentHealth - damage;
                PlayerGUI.instance.infectColor.color = colorInflict;
                gameObject.GetPhotonView().RPC("updateHealth", RpcTarget.AllBufferedViaServer, currentHealth, colorInflict.r, colorInflict.g, colorInflict.b);


                if (currentHealth <= 0)
                {
                    gameObject.GetPhotonView().RPC("playColorSound", RpcTarget.All);
                    gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, colorInflict.r, colorInflict.g, colorInflict.b);
                    currentHealth = health;
                    PlayerGUI.instance.thisPlayerHealth.ResetMeter(currentHealth);
                }
            }
            else
            {
                if ((currentHealth + damage) > health)
                {
                    currentHealth = health;
                }
                else
                {
                    currentHealth = currentHealth + damage;
                    gameObject.GetPhotonView().RPC("updateHealth", RpcTarget.AllBufferedViaServer, currentHealth, oldColor.r, oldColor.g, oldColor.b);
                }
                PlayerGUI.instance.thisPlayerHealth.ResetMeter(currentHealth);
            }
        }        
    }

    [PunRPC]
    void playerColorChange(float r, float g, float b)
    {       
        playerColor = gameObject.GetComponentInChildren<MeshRenderer>().material;
        Color c = new Color(r, g, b);
        playerColor.color = c;
        color = c;

        //PlayerGUI.instance.colorBlockAnim.enabled = false;
        //PlayerGUI.instance.colorBlockAnim.enabled = true;
        //PlayerGUI.instance.colorBlockAnim.Play("ColorChange");
        testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    #region rpc sounds

    IEnumerator stepcycle()
    {
        walkcycle = true;     
        if (playerAnim.GetBool("walking") == true && playerAnim.GetBool("grounded") == true)
        {
            gameObject.GetPhotonView().RPC("playStepSound", RpcTarget.All);
            yield return new WaitForSeconds(.5f);
            StartCoroutine(stepcycle());
        }
        else
        {
            walkcycle = false;
        }
    }

    [PunRPC]
    void playColorSound()
    {
        source.PlayOneShot(colorChange);
    }

    [PunRPC]
    void playShootSound()
    {
        source.PlayOneShot(shoot);
    }

    [PunRPC]
    void playJumpSound()
    {
        source.PlayOneShot(jump);
    }

    [PunRPC]
    void playStepSound()
    {
        source.PlayOneShot(footstep);
    }


    [PunRPC]
    void playDamageSound()
    {
        source.PlayOneShot(takeDamage);
    }
    #endregion
}
