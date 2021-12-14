using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class Player : ActiveActor
{
    public CharacterClass myClass;
    
    float speed = 7f;
    float jumpHeight = 7f;

    int currentSpecial;
    int special = 100;
    bool specialCool = false;

    Meter healthBar;
    Meter specialBar;

    public GameObject hand;
    Animator handAnim;
    public MyWeapon myWeapon;
    int currentWepID;

    public bool grounded;
    public LayerMask floor;
    public AudioClip jump;

    [HideInInspector]
    public PhotonView pv;
    Photon.Realtime.Player otherPlayer;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public override void Awake()
    {
        base.Awake();
        pv = gameObject.GetPhotonView();
        handAnim = hand.GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();
        getClass();
        
        currentHealth = health;
        currentSpecial = special;
        rb.velocity = new Vector2(0, 0);

        if (gameObject.GetPhotonView().IsMine)
        {
            currentWepID = PhotonManager.instance.weaponID;
            myWeapon.currentWeapon = PhotonManager.instance.weaponList[currentWepID];
            actTwoAuto = myWeapon.currentWeapon.auto;
            myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;

            healthBar = PlayerGUI.instance.thisPlayerHealth;
            specialBar = PlayerGUI.instance.thisPlayerCharge;

            healthBar.SetMax(health);
            healthBar.SetCurrent(currentHealth);

            specialBar.SetMax(special);
            specialBar.SetCurrent(currentSpecial);
        }
    }

    void getClass()
    {
        if (pv.IsMine)
        {
            playerProperties.Add("playerClass", PhotonManager.instance.getPlayerClass());

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            myClass = PhotonManager.instance.classList[PhotonManager.instance.getPlayerClass()];
        }
        else
        {
            otherPlayer = pv.Owner;

            myClass = PhotonManager.instance.classList[(int)otherPlayer.CustomProperties["playerClass"]];
        }

        anim.runtimeAnimatorController = myClass.animations;
        speed = 4 + myClass.speed;
        jumpHeight = 11 + (myClass.jumpHeight / 2);
        health = (myClass.hp * 20);
    }

    public override void Update()
    {
        if (pv.IsMine)
        {
            base.Update();
        }
    }

    public override void FixedUpdate()
    {
        if (pv.IsMine)
        {
            base.FixedUpdate();

            if (healthBar && hasHealth)
            {
                healthBar.SetCurrent(currentHealth);
            }
            if (specialBar)
            {
                specialBar.SetCurrent(currentSpecial);
            }
        }
    }

    public override void GetMovement()
    {
        if (rb.velocity.y == 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (pv.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            gameObject.transform.Translate((h * h) * speed * Time.deltaTime, 0, 0);

            if (h < 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            }
            else if (h > 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            }
        } 
    }

    #region Input Actions

    public override void actionOne()
    {
        if (rb.velocity.y == 0)
        {
            StartCoroutine(Jump());
        }
    }

    public override void actionTwo()
    {
        if (myWeapon.isWeapon && myWeapon.cooldown == false)
        {
            pv.RPC("makeShot", RpcTarget.All);
            ApplyRecoil(myWeapon.currentWeapon.recoil, -transform.right);
        }
    }

    public override void actionThree()
    {
        pv.RPC("playerSpecial", RpcTarget.All, 10, 3f);
    }

    public override void actionFour()
    {
        pv.RPC("playerDamage", RpcTarget.All, Vector3.zero, 20, pv.ViewID);
    }

    public override void leftAction()
    {
        pv.RPC("nextWeapon", RpcTarget.AllBuffered);
        //source.PlayOneShot(weaponList[currentWeapon].loadSound);
    }

    public override void rightAction()
    {
        pv.RPC("prevWeapon", RpcTarget.AllBuffered);
        //source.PlayOneShot(weaponList[currentWeapon].loadSound);
    }

    #endregion

    [PunRPC]
    public void nextWeapon()
    {
        currentWepID++;
        if (currentWepID == PhotonManager.instance.weaponList.Count)
        {
            currentWepID = 0;
        }

        myWeapon.currentWeapon = PhotonManager.instance.weaponList[currentWepID];
        actTwoAuto = myWeapon.currentWeapon.auto;
        myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;
    }

    [PunRPC]
    public void prevWeapon()
    {
        currentWepID--;
        if (currentWepID < 0)
        {
            currentWepID = PhotonManager.instance.weaponList.Count - 1;
        }

        myWeapon.currentWeapon = PhotonManager.instance.weaponList[currentWepID];
        actTwoAuto = myWeapon.currentWeapon.auto;
        myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;
    }

    [PunRPC]
    public void makeShot()
    {
        StartCoroutine(myWeapon.Shoot(pv.ViewID));
    }

    [PunRPC]
    public void playerDamage(Vector3 knockDir, int damage, int attacker)
    {
        StartCoroutine(Hurt(knockDir, damage, attacker));
    }

    [PunRPC]
    public void playerSpecial(int specialCost, float specialCooldown)
    {
        if (specialCool == false)
        {
            StartCoroutine(specialUse(specialCost, specialCooldown));
        }
    }

    [PunRPC]
    public void playerDies(string killer)
    {
        if (pv.IsMine)
        {
            PlayerGUI.instance.playerDeath(true);
        }
        StartCoroutine(actorDies(killer));
    }

    public override Vector2 getSpawnPoint()
    {
        if (pv.IsMine)
        {
            healthBar.ResetMeter(health);
            PlayerGUI.instance.playerDeath(false);
        }
        return GameManager.instance.getSpawn().transform.position;
    }

    public virtual IEnumerator specialUse(int specialCost, float specialCooldown)
    {
        specialCool = true;
        source.PlayOneShot(jump);
        Debug.Log(gameObject.name + " used their special");
        currentSpecial -= specialCost;

        yield return new WaitForSeconds(specialCooldown);
        source.PlayOneShot(jump);
        specialCool = false;
    }

    IEnumerator Jump()
    {
        source.PlayOneShot(jump);
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

        yield return new WaitUntil(() => Input.GetAxis("Vertical") == 0);     
    }

    public void ApplyRecoil(float recoil, Vector2 direction)
    {
        //rb.velocity = (rb.velocity / 2);
        //rb.AddForce(direction * recoil, ForceMode2D.Impulse);
    }

    public override void SpriteProcessing()
    {
        if (Input.GetAxis("Horizontal") != 0 && rb.velocity.y == 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (rb.velocity.y != 0)
        {
            anim.SetBool("Jumping", true);
        }
        else
        {
            anim.SetBool("Jumping", false);
        }

        if (handAnim == true)
        {
            if (Input.GetKey(KeyCode.UpArrow) == true && Input.GetKey(KeyCode.DownArrow) == false)
            {
                handAnim.SetBool("LookUp", true);
                handAnim.SetBool("LookDown", false);
            }
            else if (Input.GetKey(KeyCode.UpArrow) == false && Input.GetKey(KeyCode.DownArrow) == true)
            {
                if (grounded == false)
                {
                    handAnim.SetBool("LookUp", false);
                    handAnim.SetBool("LookDown", true);
                }
            }
            else
            {
                handAnim.SetBool("LookUp", false);
                handAnim.SetBool("LookDown", false);
            }

            if (grounded == true && handAnim.GetBool("LookDown") == true)
            {
                handAnim.SetBool("LookDown", false);
            }
        }
    }
}
