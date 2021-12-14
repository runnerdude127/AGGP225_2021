using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PlayerManager : Character
{
    #region GUI

    public Meter healthBar;
    public Meter chargeBar;

    #endregion

    #region Player Color

    public Color color;
    public Color testColor;
    public Material playerColor;
    // opposite : new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);

    Color oldColor;
    Color colorInflict;

    #endregion

    #region Weapon Management

    public GameObject myNose;
    public Transform shootPos;
    public GameObject rayShot;

    #endregion

    #region Camera Management

    CameraManagaer cam;
    Vector3 camDir;

    #endregion

    #region Sound Management

    // Server-side
    bool walkcycle = false;
    public AudioClip footstep;
    public AudioClip jump;
    public AudioClip colorChange;
    public AudioClip takeDamage;
    public AudioClip shoot;

    // Client-side
    public AudioClip chargeSound;
    public AudioClip fullyCharged;
    public AudioClip chargeDeplete;
    public AudioClip noCharge;
    public AudioClip hitConfirm;

    #endregion

    public override void Awake()
    {
        base.Awake();

        PlayerGUI.instance.thisPlayerCharge.SetMax(charge);
        PlayerGUI.instance.thisPlayerHealth.SetMax(health);

        playerColor = gameObject.GetComponentInChildren<MeshRenderer>().material;
        cam = gameObject.GetComponent<CameraManagaer>();
        

        if (gameObject.GetPhotonView().IsMine)
        {  
            color = PhotonManager.instance.myColor;
            Debug.Log("r: " + color.r + "g: " + color.g + "b: " + color.b);
            testColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;           
            gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);
            gameObject.GetPhotonView().RPC("playerTagUpdate", RpcTarget.AllBufferedViaServer, PhotonManager.instance.myUsername);

            ChatroomManager.instance.consoleMessage("JoinGameMessage");

            myNameTag.GetComponentInChildren<Canvas>().enabled = false;
            myNose.GetComponent<MeshRenderer>().enabled = false;
            myName.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetPhotonView().RPC("setShoot", RpcTarget.All);
            PlayerGUI.instance.thisPlayerHealth.SetCurrent(currentHealth);
            PlayerGUI.instance.thisPlayerCharge.SetCurrent(currentCharge);          

            PlayerGUI.instance.colorUI.color = color;
            PlayerGUI.instance.healthColor.color = color;
            playerColor.color = color;

            camDir = cam.GetCameraDirection();
            if (cam != null)
            {
                cam.OnStartFollowing();
            }

            if (cooldownCharge)
            {
                PlayerGUI.instance.chargeColor.color = new Color(.5f, .5f, .5f);
            }
            else
            {
                PlayerGUI.instance.chargeColor.color = new Color(1, 1, 1);
            }

            if (PlayerGUI.instance.playerKeyInput == true)              // If the player is allowed to use their keyboard for in-game processing...
            {
                moveBy();
            }
            else                                                        // Otherwise, KILL ALL MOVEMENT!!
            {
                killMovement();
            }

            if (PlayerGUI.instance.playerMouseInput == true)            // If the player is allowed to use their mouse for in-game processing...
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, camDir.y, gameObject.transform.eulerAngles.z); // Player points towards mouse direction.

                if (Input.GetKeyDown(KeyCode.Mouse0))                   // Left Click
                {
                    chargeAction();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))                   // Right Click
                {
                    gameObject.GetPhotonView().RPC("playerHurt", RpcTarget.All, 10, testColor.r, testColor.g, testColor.b);
                }
            }
        }
    }

    public override void AnimationHandler()
    {
        myAnim.SetBool("grounded", characterController.isGrounded);
        if (horizontal != 0 || vertical != 0)
        {
            myAnim.SetBool("walking", true);
            if (walkcycle == false)
            {
                StartCoroutine(stepcycle());
            }
        }
        else
        {
            myAnim.SetBool("walking", false);
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                myAnim.SetBool("jumping", true);
            }
            else
            {
                myAnim.SetBool("jumping", false);
            }
        }
    }

    public override void onJumping()
    {
        gameObject.GetPhotonView().RPC("playJumpSound", RpcTarget.All);
    }

    #region Health Functions
    public override void onDamaged()
    {
        myAnim.ResetTrigger("damage");
        myAnim.SetTrigger("damage");

        gameObject.GetPhotonView().RPC("playDamageSound", RpcTarget.All);
        PlayerGUI.instance.infectColor.color = colorInflict;
        gameObject.GetPhotonView().RPC("updateHealth", RpcTarget.AllBufferedViaServer, colorInflict.r, colorInflict.g, colorInflict.b);
    }

    public override void onHealed()
    {
        gameObject.GetPhotonView().RPC("updateHealth", RpcTarget.AllBufferedViaServer, oldColor.r, oldColor.g, oldColor.b);
        PlayerGUI.instance.thisPlayerHealth.ResetMeter(currentHealth);
    }

    public override void healthDepleted()
    {
        gameObject.GetPhotonView().RPC("playColorSound", RpcTarget.All);
        gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, colorInflict.r, colorInflict.g, colorInflict.b);
        currentHealth = health;
        PlayerGUI.instance.thisPlayerHealth.ResetMeter(currentHealth);
    }

    #endregion

    #region Charge Functions
    public override void chargeDepleted()
    {
        source.PlayOneShot(chargeDeplete);
    }

    public override void chargeFilled()
    {
        source.PlayOneShot(fullyCharged);
    }

    public override void commitChargeAction()
    {
        shootWeapon(color.r, color.g, color.b);
        PlayerGUI.instance.thisPlayerCharge.ResetMeter(currentCharge);
    }

    public override void invalidChargeAction()
    {
        source.PlayOneShot(noCharge);
    }

    #endregion

    public void playHitConfirmSound()
    {
        source.PlayOneShot(hitConfirm);
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
        myAnim.ResetTrigger("shoot");
        myAnim.SetTrigger("shoot");
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

    

    [PunRPC]
    void playerTagUpdate(string nameSet)
    {
        headHealthBar.SetMax(health);
        headHealthBar.SetMainColor(playerColor.color);
        this.myName.text = nameSet;
    }

    [PunRPC]
    void updateHealth(float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        headHealthBar.SetMainColor(playerColor.color);
        headHealthBar.SetLossColor(c);
    }

    [PunRPC]
    public void playerHurt(int damage, float r, float g, float b)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            oldColor = new Color(PlayerGUI.instance.infectColor.color.r, PlayerGUI.instance.infectColor.color.g, PlayerGUI.instance.infectColor.color.b);
            colorInflict = new Color(r, g, b);
            
            if (playerColor.color != colorInflict)
            {
                changeHealth(damage * -1);
            }
            else
            {
                changeHealth(damage / 2);
            }
            /*
            myAnim.ResetTrigger("damage");
            myAnim.SetTrigger("damage");
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
                    currentHealth = currentHealth + (damage / 2);
                    gameObject.GetPhotonView().RPC("updateHealth", RpcTarget.AllBufferedViaServer, currentHealth, oldColor.r, oldColor.g, oldColor.b);
                }
                PlayerGUI.instance.thisPlayerHealth.ResetMeter(currentHealth);
            }
            */
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
        if (myAnim.GetBool("walking") == true && myAnim.GetBool("grounded") == true)
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
