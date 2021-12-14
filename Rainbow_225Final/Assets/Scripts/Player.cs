using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class Player : Actor
{
    public CharacterClass myClass;
    
    float speed = 7f;
    float jumpHeight = 7f;

    int currentSpecial;
    int special = 100;
    bool specialCool = false;

    Meter healthBar;
    Meter specialBar;

    public GameObject myHand;
    
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

    void Update()
    {
        SpriteProcessing();
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
            if (PlayerGUI.instance.playerInput)
            {
                GetInstant();
            }
        }
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            if (PlayerGUI.instance.playerInput)
            {
                GetInput();
            }

            if (healthBar && hasHealth)
            {
                healthBar.SetCurrent(currentHealth);
            }
            if (specialBar)
            {
                specialBar.SetCurrent(currentSpecial);
            }
        }
        //GetInput();
    }

    void GetInstant()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (rb.velocity.y == 0)
            {
                StartCoroutine(Jump());
            }
        }

        PlayerHand thisHand = myHand.GetComponent<PlayerHand>();
        MyWeapon myWeapon = thisHand.weapon.GetComponent<MyWeapon>();

        if (myWeapon.currentWeapon.auto == false)
        {
            if (Input.GetKeyDown(KeyCode.X) && myWeapon.isWeapon && myWeapon.cooldown == false)
            {
                pv.RPC("makeShot", RpcTarget.All);
                Debug.Log("one");
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.X) && myWeapon.isWeapon && myWeapon.cooldown == false)
            {
                pv.RPC("makeShot", RpcTarget.All);
                Debug.Log("one");
            }
        }
        if (myWeapon && thisHand.anim == true)
        {
            thisHand.SpriteProcessing();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            pv.RPC("playerDamage", RpcTarget.All, Vector3.zero, 20, pv.ViewID);
            //StartCoroutine(Hurt(Vector3.zero, 20));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            pv.RPC("playerSpecial", RpcTarget.All, 10, 3f);
            //StartCoroutine(Special(10, 3));
        }
    }

    [PunRPC]
    public void makeShot()
    {
        PlayerHand thisHand = myHand.GetComponent<PlayerHand>();
        StartCoroutine(thisHand.Shoot());
    }


    void GetInput()
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

    public IEnumerator specialUse(int specialCost, float specialCooldown)
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

    void SpriteProcessing()
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
    }
}
