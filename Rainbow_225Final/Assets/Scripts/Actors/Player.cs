using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/*public class Player : ActiveActor
{
    public CharClass myClass;

    float speed = 7f;
    float jumpHeight = 7f;
    public bool canJump = true;
    Skill mySkill;
    [SerializeField]
    float smoothTime = .1f;
    public float myHorizontalMovement = 0f;
    public float myVerticalMovement = 0f;
    float inputVelocity;
    bool waitingForLand = false;
    bool stepProcess = false;
    float lastMovement = 0;

    public int currentSpecial;
    int special = 100;

    Meter healthBar;
    Meter specialBar;

    public List<int> weaponInventory;
    bool emptyHanded = true;

    public GameObject hand;
    Animator handAnim;
    public MyWeapon myWeapon;
    int currentWepID;

    public bool flippingOnly = false;
    public bool climbing = false;

    public GameObject jumpPoof;
    public GameObject landPoof;

    public AudioClip jump;
    public AudioClip land;
    public AudioClip step1;
    public AudioClip step2;
    public AudioClip switchWeapon;

    public Team myTeam;

    [HideInInspector]
    public PhotonView pv;
    Photon.Realtime.Player otherPlayer;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public override void Awake()
    {
        base.Awake();
        pv = gameObject.GetPhotonView();
        handAnim = hand.GetComponent<Animator>();
        mySkill = gameObject.GetComponent<Skill>();
        weaponInventory = new List<int>();

        hand.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
        getClass();

        currentHealth = health;
        currentSpecial = special;
        rb.velocity = new Vector2(0, 0);

        if (pv.IsMine)
        {
            PlayerGUI.instance.portraitIcon.sprite = myClass.bigIcon;

            healthBar = PlayerGUI.instance.thisPlayerHealth;
            specialBar = PlayerGUI.instance.thisPlayerCharge;

            healthBar.SetMax(health);
            healthBar.SetCurrent(currentHealth);

            specialBar.SetMax(special);
            specialBar.SetCurrent(currentSpecial);

            pv.RPC("spawn", RpcTarget.All, true);
        }
        spriteRend.material.SetColor("Color_54667E5E", myTeam.teamColor);
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
        health = (myClass.health * 20);
        actThreeAuto = myClass.autoSkill;
    }

    //[PunRPC]
    //public void joinTeam(int teamID)
    //{
    //    Team teamAssigned = GameManager.instance.getTeam(teamID);
    //    teamAssigned.members.Add(this);
    //    myTeam = teamAssigned;
    //}

    public override void Update()
    {
        if (pv.IsMine)
        {
            base.Update();
        }
        else
        {
            checkBlinks();
        }

        if (dead == false && myTeam != null)
        {
            spriteRend.material.SetInt("Boolean_E47E2B67", 1);
            spriteRend.material.SetColor("Color_54667E5E", myTeam.teamColor);
        }

        if (grounded == false && waitingForLand == false)
        {
            StartCoroutine(waitForLand());
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
        if (pv.IsMine)
        {
            if (climbing)
            {
                GetClimbing();
            }
            else
            {
                GetWalking();
            }


            if (controls.Base.Move.ReadValue<float>() < 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            }
            else if (controls.Base.Move.ReadValue<float>() > 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            }
            /*if (myHorizontalMovement < 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            }
            else if (myHorizontalMovement > 0)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            }
        }
    }

    void GetWalking()
    {
        float horizontalMove = controls.Base.Move.ReadValue<float>();
        myHorizontalMovement = Mathf.SmoothDamp(myHorizontalMovement, horizontalMove, ref inputVelocity, smoothTime);
        if (horizontalMove != 0 && stepProcess == false && grounded == true)
        {
            StartCoroutine(walkSound());
        }
        if (flippingOnly == false && faceTouch == false)
        {
            if (stuck)
            {
                if (horizontalMove != lastMovement)
                {
                    if (horizontalMove != 0)
                    {
                        nudging = true;
                    }
                    lastMovement = horizontalMove;
                }
                else
                {
                    nudging = false;
                }
            }
            else
            {
                gameObject.transform.Translate((myHorizontalMovement * myHorizontalMovement) * speed * Time.deltaTime, 0, 0);
            }
        }
        else if (faceTouch)
        {
            //rb.velocity = new Vector2(0rb.velocity.x / 2, rb.velocity.y);
        }
    }

    void GetClimbing()
    {
        float verticalMove = controls.Base.Climb.ReadValue<float>();
        //Debug.Log(verticalMove);
        myVerticalMovement = Mathf.SmoothDamp(myVerticalMovement, verticalMove, ref inputVelocity, smoothTime);
        if (flippingOnly == false && faceTouch == true)
        {
            gameObject.transform.Translate(0, myVerticalMovement * speed * Time.deltaTime, 0);
        }
    }

    IEnumerator walkSound()
    {
        stepProcess = true;
        if (controls.Base.Move.ReadValue<float>() != 0 && stepProcess == true && grounded == true)
        {
            source.PlayOneShot(step1);
            yield return new WaitForSeconds(1 / speed);
        }
        else
        {
            stepProcess = false;
        }
        if (controls.Base.Move.ReadValue<float>() != 0 && stepProcess == true && grounded == true)
        {
            source.PlayOneShot(step2);
            yield return new WaitForSeconds(1 / speed);
        }
        else
        {
            stepProcess = false;
        }
        if (controls.Base.Move.ReadValue<float>() != 0 && stepProcess == true && grounded == true)
        {
            StartCoroutine(walkSound());
        }
        else
        {
            stepProcess = false;
        }
    }



    #region Input Actions

    public override void actionOne()
    {
        if (grounded == true && canJump)
        {
            StartCoroutine(Jump());
        }
    }

    public override void actionTwo()
    {
        if (myWeapon.isWeapon && myWeapon.cooldown == false && emptyHanded == false)
        {
            StartCoroutine(myWeapon.Shoot(true, pv.ViewID));
        }
    }

    public override void actionThree(bool keyPressed)
    {
        pv.RPC("playerAutoSpecial", RpcTarget.All, myClass.skillCost, keyPressed);
    }

    public override void actionThree()
    {
        pv.RPC("playerSpecial", RpcTarget.All, myClass.skillCost);
    }

    public override void actionFour()
    {
        pv.RPC("playerDamage", RpcTarget.All, Vector3.zero, 20, pv.ViewID, .1f);
    }

    public override void leftAction()
    {
        if (!emptyHanded)
        {
            pv.RPC("nextWeapon", RpcTarget.AllBuffered);
            if (weaponInventory.Count != 1)
            {
                source.PlayOneShot(switchWeapon);
            }
        }
    }

    public override void rightAction()
    {
        if (!emptyHanded)
        {
            pv.RPC("prevWeapon", RpcTarget.AllBuffered);
            if (weaponInventory.Count != 1)
            {
                source.PlayOneShot(switchWeapon);
            }
        }
    }

    #endregion

    [PunRPC]
    public void setWeapon(bool empty, int weaponID)
    {
        if (empty == true)
        {
            currentWepID = 0;
            myWeapon.currentWeapon = PhotonManager.instance.weaponList[currentWepID];
            myWeapon.spriteRend.enabled = false;
        }
        else
        {
            myWeapon.spriteRend.enabled = true;
            myWeapon.currentWeapon = PhotonManager.instance.weaponList[weaponInventory[currentWepID]];
            actTwoAuto = myWeapon.currentWeapon.auto;
            myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;
        }
    }

    [PunRPC]
    public void nextWeapon()
    {
        currentWepID++;
        if (currentWepID == weaponInventory.Count)
        {
            currentWepID = 0;
        }

        myWeapon.currentWeapon = PhotonManager.instance.weaponList[weaponInventory[currentWepID]];
        actTwoAuto = myWeapon.currentWeapon.auto;
        myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;
    }

    [PunRPC]
    public void prevWeapon()
    {
        currentWepID--;
        if (currentWepID < 0)
        {
            currentWepID = weaponInventory.Count - 1;
        }

        myWeapon.currentWeapon = PhotonManager.instance.weaponList[weaponInventory[currentWepID]];
        actTwoAuto = myWeapon.currentWeapon.auto;
        myWeapon.spriteRend.sprite = myWeapon.currentWeapon.sprite;
    }

    [PunRPC]
    public void playerDamage(Vector3 knockDir, int damage, int attacker, float hitStun)
    {
        StartCoroutine(Hurt(knockDir, damage, attacker, hitStun));
        if (pv.IsMine && invulnerable == false && hitStun >= .05f)
        {
            StartCoroutine(GameManager.instance.mainCam.CameraShake(hitStun, damage * .01f));
        }
    }

    [PunRPC]
    public void playerSpecial(int specialCost)
    {
        if (mySkill.doingSkill == false)
        {
            Debug.Log(gameObject.name + " used their special");
            mySkill.activateSkill(myClass, specialCost);
            currentSpecial -= specialCost;
        }
    }

    [PunRPC]
    public void playerAutoSpecial(int specialCost, bool autoSkill)
    {
        if (autoSkill == true)
        {
            if (mySkill.doingSkill == false)
            {
                Debug.Log(gameObject.name + " started their special");
                mySkill.activateSkill(myClass, specialCost);
            }
        }
        else
        {
            if (mySkill.doingSkill == true)
            {
                Debug.Log(gameObject.name + " stopped their special");
                mySkill.stopSkill();
            }
        }
    }

    [PunRPC]
    public void playerDies(string killer, float kx, float ky, float kz)
    {
        //drop weapon holding
        if (pv.IsMine)
        {
            PlayerGUI.instance.playerDeath(true);
        }
        hand.SetActive(false);
        StartCoroutine(actorDies(killer, new Vector3(kx, ky, kz)));
    }

    [PunRPC]
    void spawn(bool firstSpawn)
    {
        StartCoroutine(Respawn(firstSpawn));
    }

    public override IEnumerator Respawn(bool firstSpawn)
    {
        StartCoroutine(base.Respawn(firstSpawn));
        yield return new WaitUntil(()=> isRespawning == false);

        if (myTeam != null)
        {
            spriteRend.material.SetInt("Boolean_E47E2B67", 1);
            spriteRend.material.SetColor("Color_54667E5E", myTeam.teamColor);
        }
        else
        {
            spriteRend.material.SetInt("Boolean_E47E2B67", 0);
        }

        currentWepID = 0;
        emptyHanded = true;
        weaponInventory.Clear();

        // give starting weapons ( if any )

        if (weaponInventory.Count > 0)
        {
            hand.SetActive(true);
            emptyHanded = false;
            pv.RPC("setWeapon", RpcTarget.AllBuffered, emptyHanded, weaponInventory[currentWepID]);
            StartCoroutine(myWeapon.reload());
        }

        if (myWeapon.reloading == true)
        {
            myWeapon.reloading = false;
        }
        StartCoroutine(invulnFrames(0f));
    }

    //public override Vector2 getSpawnPoint()
    //{
    //    if (pv.IsMine)
    //    {
    //        healthBar.ResetMeter(health);
    //        PlayerGUI.instance.playerDeath(false);
    //    }
    //    return GameManager.instance.getSpawn(this).transform.position;
    //}

    IEnumerator Jump()
    {
        source.PlayOneShot(jump);
        Instantiate(jumpPoof, new Vector3(transform.position.x, transform.position.y, -1) + (Vector3.down * col.bounds.extents.y), Quaternion.Euler(-90, 0, 0));
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

        yield return new WaitUntil(() => grounded == true);     
    }

    IEnumerator waitForLand()
    {
        waitingForLand = true;
        yield return new WaitUntil(() => grounded == true);
        landing();
    }

    void landing()
    {
        source.PlayOneShot(land);
        Instantiate(landPoof, new Vector3(transform.position.x, transform.position.y, -1) + (Vector3.down * col.bounds.extents.y), Quaternion.Euler(-90, 0, 0));
        waitingForLand = false;
    }

    public void ApplyRecoil(float recoil, Vector2 direction)
    {
        if (grounded)
        {
            if (recoil > 9)
            {
                rb.AddForce(direction * recoil, ForceMode2D.Impulse);
            }
        }
        else
        {
            rb.AddForce(direction * recoil, ForceMode2D.Impulse);
        }
        
        if (recoil != 0)
        {
            if (handAnim.GetBool("LookDown"))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            }
            else
            {
                if (!grounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x / 3, rb.velocity.y);
                }
                else if (grounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y);
                }
            }
        }
    }

    public bool hasWeapon(int weaponCheck)
    {
        if (weaponInventory.Contains(weaponCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void collectWeapon(int weaponCollected)
    {
        pv.RPC("addWeapon", RpcTarget.AllBuffered, weaponCollected);
    }

    [PunRPC]
    void addWeapon(int weaponCollected)
    {
        weaponInventory.Add(weaponCollected);
        GameManager.instance.source.PlayOneShot(PhotonManager.instance.weaponList[weaponCollected].callout);
        if (emptyHanded)
        {
            emptyHanded = false;
            hand.SetActive(true);
            pv.RPC("setWeapon", RpcTarget.AllBuffered, emptyHanded, weaponCollected);
        }
    }

    public override void SpriteProcessing()
    {
        anim.SetBool("Dead", dead);
        anim.SetBool("Grounded", grounded);
        if (dead == false)
        {
            if (controls.Base.Move.ReadValue<float>() != 0 && rb.velocity.y == 0)
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
                if (controls.Base.LookUp.ReadValue<float>() != 0 && controls.Base.LookDown.ReadValue<float>() == 0)
                {
                    handAnim.SetBool("LookUp", true);
                    handAnim.SetBool("LookDown", false);
                }
                else if (controls.Base.LookUp.ReadValue<float>() == 0 && controls.Base.LookDown.ReadValue<float>() != 0)
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
}*/
