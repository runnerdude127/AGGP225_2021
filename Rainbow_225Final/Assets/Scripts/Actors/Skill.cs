using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    Player myPlayer;
    public bool touchingSomething;
    bool waiting = false;
    public bool doingSkill = false;

    Animator anim;

    AudioSource source;

    public AudioClip skillReady;

    public AudioClip ambCharge;
    public AudioClip ambDash;
    public AudioClip ambInterupt;

    private void Awake()
    {
        myPlayer = gameObject.GetComponent<Player>();
        source = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
    }
    public void activateSkill(CharacterClass myClass)
    {
        if (doingSkill == false)
        {
            if (myClass.name == "Vermili")
            {
                Debug.Log("Vermili");
            }
            else if (myClass.name == "Ambers")
            {
                StartCoroutine(ambersSkill());
                Debug.Log("Ambers");
            }
            else if (myClass.name == "Canary")
            {
                Debug.Log("Canary");
            }
            else if (myClass.name == "Miretreuse")
            {
                Debug.Log("Miretreuse");
            }
            else if (myClass.name == "Seenobi")
            {
                Debug.Log("Seenobi");
            }
            else if (myClass.name == "Aquack")
            {
                Debug.Log("Aquack");
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

    IEnumerator ambersSkill()
    {
        doingSkill = true;
        myPlayer.canJump = false;


        float val = 30;
        float lol = myPlayer.rb.gravityScale;

        source.PlayOneShot(ambCharge);
        myPlayer.controllable = false;
        myPlayer.rb.gravityScale = 0;
        myPlayer.rb.velocity = new Vector2(0, 0);

        myPlayer.rb.AddForce(myPlayer.transform.up * (val / 10) * myPlayer.rb.mass, ForceMode2D.Impulse);
        myPlayer.rb.AddForce(myPlayer.transform.right * (val / 20) * myPlayer.rb.mass, ForceMode2D.Impulse);
        anim.Play("Charge");
        yield return new WaitForSeconds(.25f);
        anim.Play("DashTransition");
        yield return new WaitForSeconds(.05f);
        myPlayer.rb.velocity = new Vector2(0, 0);
        StartCoroutine(waitForSecs(.4f));
        myPlayer.rb.AddForce(myPlayer.transform.right * val * myPlayer.rb.mass, ForceMode2D.Impulse);
        anim.Play("Dash");
        source.PlayOneShot(ambDash);
        while (touchingSomething == false && waiting == true)
        {
            yield return new WaitForSeconds(0f);
        }
        myPlayer.rb.velocity = (myPlayer.rb.velocity / val);
        if (touchingSomething == true)
        {
            anim.Play("DashTransition");
            source.Stop();
            source.PlayOneShot(ambInterupt);
            myPlayer.rb.AddForce(-myPlayer.transform.right * (val / 10) * myPlayer.rb.mass, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.05f);
        }
        anim.Play("Walk");

        myPlayer.rb.AddForce(myPlayer.transform.up * (val / 10) * myPlayer.rb.mass, ForceMode2D.Impulse);
        myPlayer.rb.gravityScale = lol;
        myPlayer.controllable = true;

        myPlayer.canJump = true;
        yield return new WaitForSeconds(1f);
        source.PlayOneShot(skillReady);
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
