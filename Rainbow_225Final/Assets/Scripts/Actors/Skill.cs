using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Skill : NetworkBehaviour
{
    PlayerMIRROR myPlayer;
    public bool touchingSomething;
    bool waiting = false;
    public bool doingSkill = false;

    public bool invulnerable;
    public int reflectState = 0;
    const int rflct_NONE = 0;
    const int rflct_DIR = 1;
    const int rflct_ALL = 2;

    Collider2D playerCol;
    BoxCollider2D colBox;
    Vector2 colOriginSize;
    Vector2 colOriginOffset;
    float baseGrav;

    bool vermiliSkillActive = false;
    bool ambersSkillActive = false;
    bool canarySkillActive = false;
    bool miretreuseSkillActive = false;
    bool seenobiSkillActive = false;
    bool aquackSkillActive = false;
    bool zuliqueSkillActive = false;
    bool maunkSkillActive = false;

    public GameObject reflectBox;
    GameObject currentRFB;
    public int pillID = 6;
    public int snareID = 8;

    Animator anim;

    AudioSource source;

    public AudioClip skillReady;

    public AudioClip ambCharge;
    public AudioClip ambDash;
    public AudioClip ambInterupt;

    public AudioClip canReflect;

    private void Awake()
    {
        myPlayer = gameObject.GetComponent<PlayerMIRROR>();
        source = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
        playerCol = gameObject.GetComponent<Collider2D>();
        colBox = myPlayer.GetComponent<BoxCollider2D>();
        colOriginSize = colBox.size;
        colOriginOffset = colBox.offset;
    }

    private void Start()
    {
        baseGrav = myPlayer.rb.gravityScale;
    }

    private void Update()
    {
        checkPassive();
    }

    void checkPassive()
    {
        if (myPlayer.myClass.name == "Vermili")
        {
            //Debug.Log("Vermili");
        }
        else if (myPlayer.myClass.name == "Ambers")
        {
            //Debug.Log("Ambers");
        }
        else if (myPlayer.myClass.name == "Canary")
        {
            //Debug.Log("Canary");
        }
        else if (myPlayer.myClass.name == "Miretreuse")
        {
            //Debug.Log("Miretreuse");
            if (myPlayer.faceTouch)
            {
                myPlayer.climbing = true;
                myPlayer.rb.gravityScale = 0;
                myPlayer.rb.velocity = myPlayer.rb.velocity / 1.5f;
            }
            else
            {
                myPlayer.climbing = false;
                myPlayer.rb.gravityScale = baseGrav;
            }
        }
        else if (myPlayer.myClass.name == "Seenobi")
        {
            //Debug.Log("Seenobi");
        }
        else if (myPlayer.myClass.name == "Aquack")
        {
            //Debug.Log("Aquack");
        }
        else if (myPlayer.myClass.name == "Zulique")
        {
            //Debug.Log("Zulique");
        }
        else if (myPlayer.myClass.name == "Maunk")
        {
            //Debug.Log("Maunk");
        }
        else
        {
            Debug.LogError("This character doesn't have a skill!");
        }  
    }

    public void activateSkill(CharClass myClass, int cost)
    {
        if (doingSkill == false)
        {
            doingSkill = true;
            if (myClass.name == "Vermili")
            {
                Debug.Log("Vermili");
            }
            else if (myClass.name == "Ambers")
            {
                myPlayer.currentSpecial -= cost;
                StartCoroutine(ambersSkill());
                //Debug.Log("Ambers");
            }
            else if (myClass.name == "Canary")
            {
                canarySkill();
                //Debug.Log("Canary");
            }
            else if (myClass.name == "Miretreuse")
            {
                if (miretreuseSkillActive == false)
                {
                    myPlayer.currentSpecial -= cost;
                    StartCoroutine(miretreuseSkill(myPlayer.gameObject.name));
                }
            }
            else if (myClass.name == "Seenobi")
            {
                Debug.Log("Seenobi");
            }
            else if (myClass.name == "Aquack")
            {
                if (aquackSkillActive == false)
                {
                    myPlayer.currentSpecial -= cost;
                    StartCoroutine(aquackSkill(myPlayer.gameObject.name));
                }
            }
            else if (myClass.name == "Zulique")
            {
                Debug.Log("Zulique");
            }
            else if (myClass.name == "Maunk")
            {
                Debug.Log("Maunk");
            }
            else
            {
                Debug.LogError("This character doesn't have a skill!");
            }
        }
    }

    public void stopSkill()
    {
        doingSkill = false;
        if (canarySkillActive)
        {
            myPlayer.flippingOnly = false;
            anim.SetBool("Guarding", false);
            reflectState = rflct_NONE;
            if (currentRFB)
            {
                Destroy(currentRFB);
                colBox.size = colOriginSize;
                colBox.offset = colOriginOffset;
            }
            canarySkillActive = false;
        }
    }

    public void reflected()
    {
        if (canarySkillActive)
        {
            source.PlayOneShot(canReflect);
            anim.SetTrigger("BlockedShot");
        }
    }

    IEnumerator ambersSkill()
    {
        BoxCollider2D colBox = myPlayer.GetComponent<BoxCollider2D>();
        myPlayer.canJump = false;

        float val = 30;

        myPlayer.rb.velocity = myPlayer.rb.velocity / 2;
        source.PlayOneShot(ambCharge);
        
        anim.Play("Charge");
        yield return new WaitForSeconds(.25f);
        anim.Play("DashTransition");
        yield return new WaitForSeconds(.05f);
        myPlayer.rb.velocity = new Vector2(0, 0);
        StartCoroutine(waitForSecs(.4f));

        myPlayer.controllable = false;
        myPlayer.rb.gravityScale = 0;
        myPlayer.rb.velocity = new Vector2(0, 0);
        invulnerable = true;
        ambersSkillActive = true;

        myPlayer.rb.AddForce(myPlayer.transform.right * val * myPlayer.rb.mass, ForceMode2D.Impulse);
        colBox.size = new Vector2(colOriginSize.x, colOriginSize.y / 2);
        anim.Play("Dash");
        source.PlayOneShot(ambDash);
        while (myPlayer.faceTouch == false && waiting == true)
        {
            yield return new WaitForSeconds(0f);
        }
        myPlayer.rb.velocity = (myPlayer.rb.velocity / val);
        if (myPlayer.faceTouch == true)
        {
            anim.Play("DashTransition");
            source.Stop();
            source.PlayOneShot(ambInterupt);
            myPlayer.rb.AddForce(-myPlayer.transform.right * (val / 10) * myPlayer.rb.mass, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.05f);
        }
        colBox.size = colOriginSize;
        invulnerable = false;
        ambersSkillActive = false;
        anim.Play("Walk");

        myPlayer.rb.gravityScale = baseGrav;
        myPlayer.controllable = true;

        myPlayer.canJump = true;
        yield return new WaitForSeconds(1f);
        source.PlayOneShot(skillReady);
        stopSkill();
    }

    void canarySkill()
    {
        BoxCollider2D colBox = myPlayer.GetComponent<BoxCollider2D>();
        colBox.size = new Vector2(.5f, 1);
        colBox.offset = new Vector2(-0.25f, 0);
        currentRFB = Instantiate(reflectBox, gameObject.transform);
        canarySkillActive = true;
        myPlayer.flippingOnly = true;
        anim.SetBool("Guarding", true);
        reflectState = rflct_DIR;
    }

    IEnumerator miretreuseSkill(string player)
    {
        Vector3 snarePos = transform.position;

        miretreuseSkillActive = true;
        //animate

        if (myPlayer.hasAuthority)
        {
            Vector3 playRot = transform.rotation.eulerAngles;
            for (int x = 0; x < 3; x++)
            {
                myPlayer.CmdMakeShot(8, true, player, snarePos.x, snarePos.y, 0f, playRot.x, playRot.y, playRot.z);
            }
        }

        yield return new WaitForSeconds(0.3f);
        source.PlayOneShot(skillReady);
        miretreuseSkillActive = false;
        doingSkill = false;
    }

    IEnumerator aquackSkill(string player)
    {
        Debug.Log("ENTER");
        Vector3 shotPos = transform.position;

        aquackSkillActive = true;
        //animate

        if (myPlayer.hasAuthority)
        {
            Debug.Log("HAS AUTHORITY, SENDING COMMAND");
            Vector3 playRot = transform.rotation.eulerAngles;
            myPlayer.CmdMakeShot(6, true, player, shotPos.x, shotPos.y, 0f, playRot.x, playRot.y, playRot.z);
        }
        //source.PlayOneShot(); pill shoot sound

        yield return new WaitForSeconds(0.3f);
        source.PlayOneShot(skillReady);
        aquackSkillActive = false;
        doingSkill = false;
    }

    IEnumerator waitForSecs(float waitTime) // SEX!!
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            touchingSomething = true;
            if (ambersSkillActive == true)
            {
                PlayerMIRROR ply = collision.gameObject.GetComponent<PlayerMIRROR>();
                Actor act = collision.gameObject.GetComponent<Actor>();
                if (ply)
                {
                    Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                    ply.CmdDoPlayerDamage(knockDir, 10, gameObject.name, 1);
                }
                else if (act)
                {
                    Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                    StartCoroutine(act.Hurt(knockDir, 10, gameObject.name, .1f));
                }
                else if (collision.gameObject.tag == "Reflector")
                {
                    Skill skl = collision.gameObject.GetComponentInParent<Skill>();
                    gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.x + 180);
                    skl.reflected();
                }
            }
        }    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            touchingSomething = false;
        }
    }
}
