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

    public Color color;
    public Color testColor;
    public Material playerColor;

    public GameObject nametag;
    public TextMesh myName;
    public int health = 100;
    int currentHealth;
    public Meter healthBar;

    public int charge = 100;
    int currentCharge;
    public Meter chargeBar;
    float chargeSpeed = 1;
    bool cooldownCharge = false;


    public CharacterController characterController;
    float horizontal;
    float vertical;
    float velocity;
    CameraManagaer cam;
    Vector3 camDir;
    // opposite : new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);

    public GameObject rayShot;

    AudioSource source;
    //online
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

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        characterController = gameObject.GetComponent<CharacterController>();
        playerColor = gameObject.GetComponentInChildren<MeshRenderer>().material;
        cam = gameObject.GetComponent<CameraManagaer>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();

        currentHealth = health;
        PlayerGUI.instance.thisPlayerHealth.SetMax(health);

        StartCoroutine(ChargeBack());
        currentCharge = charge;
        PlayerGUI.instance.thisPlayerCharge.SetMax(charge);

        if (gameObject.GetPhotonView().IsMine)
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;
            gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);
            gameObject.GetPhotonView().RPC("playerSetup", RpcTarget.AllBufferedViaServer, PhotonManager.instance.myUsername);
        }
    }

    private void Update()
    {
        nametag.transform.rotation = Quaternion.LookRotation(nametag.transform.position - Camera.main.transform.position);
        if (gameObject.GetPhotonView().IsMine)
        {
            PlayerGUI.instance.thisPlayerHealth.SetCurrent(currentHealth);
            PlayerGUI.instance.thisPlayerCharge.SetCurrent(currentCharge);

            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;

            //Debug.Log("grounded: " + characterController.isGrounded + " velocity: " + velocity);
            horizontal = Input.GetAxis("Horizontal") * speed;
            vertical = Input.GetAxis("Vertical") * speed;

            playerAnim.SetBool("grounded", characterController.isGrounded);
            if (horizontal == 0 && vertical == 0)
            {
                playerAnim.SetBool("walking", false);
            }
            else 
            {
                playerAnim.SetBool("walking", true);
            }




            if (characterController.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    gameObject.GetPhotonView().RPC("playJumpSound", RpcTarget.All);
                    playerAnim.Play("Jump");
                    velocity = jumpSpeed;
                }
                else
                {
                    velocity = 0;
                }

            }
            velocity += Physics.gravity.y * Time.deltaTime;

            camDir = cam.GetCameraDirection();
            if (cam != null)
            {
                cam.OnStartFollowing();
            }

            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, camDir.y, gameObject.transform.eulerAngles.z);
            characterController.Move((gameObject.transform.right * horizontal + gameObject.transform.forward * vertical + (new Vector3(0, 1, 0) * velocity)) * Time.deltaTime);

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
                //gameObject.GetPhotonView().RPC("playerHurt", RpcTarget.All, 10, testColor.r, testColor.g, testColor.b);
                gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, testColor.r, testColor.g, testColor.b);
                testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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
        source.PlayOneShot(shoot);
        Debug.DrawRay(transform.position, cam.GetCameraFacing() * 1000, Color.red, 10);
        //Debug.Log(cam.GetCameraFacing());
        GameObject shotFired = PhotonNetwork.Instantiate(rayShot.name, this.transform.position, Quaternion.Euler(camDir), 0);
        shotFired.GetPhotonView().RPC("colorSet", RpcTarget.All, r, g, b);
        RayShot myShot = shotFired.GetComponent<RayShot>();
        myShot.parentSet(this.gameObject);
    }

    public void playHitConfirmSound()
    {
        source.PlayOneShot(hitConfirm);
    }

    [PunRPC]
    void playerSetup(string nameSet)
    {
        myName = nametag.GetComponent<TextMesh>();
        this.myName.text = nameSet;
    }

    [PunRPC]
    public void playerHurt(int damage, float r, float g, float b)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            Color colorInflict = new Color(r, g, b);
            if (playerColor.color != colorInflict)
            {
                gameObject.GetPhotonView().RPC("playDamageSound", RpcTarget.All);
                currentHealth = currentHealth - damage;
                PlayerGUI.instance.infectColor.color = colorInflict;
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

        PlayerGUI.instance.colorBlockAnim.enabled = false;
        PlayerGUI.instance.colorBlockAnim.enabled = true;
        //PlayerGUI.instance.colorBlockAnim.Play("Idle");
        PlayerGUI.instance.colorBlockAnim.Play("ColorChange");
    }

    #region rpc sounds
    [PunRPC]
    void playColorSound()
    {
        source.PlayOneShot(colorChange);
    }

    [PunRPC]
    void playJumpSound()
    {
        source.PlayOneShot(jump);
    }

    [PunRPC]
    void playDamageSound()
    {
        source.PlayOneShot(takeDamage);
    }
    #endregion
}
