using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snare : DefaultBullet
{
    public int health;
    Actor snaredObject;

    public AudioClip snareSound;
    public AudioClip snareStruggle;
    Vector2 origin;

    public bool grounded;
    public bool faceTouch;
    public bool backTouch;
    public bool upTouch;

    public override void Awake()
    {
        base.Awake();
    }

    bool moveToPosition()
    {
        transform.position = origin;
        int roll;
        for (int x = 0; x < 3; x++)
        {
            roll = Random.Range(1, 5);
            if (roll == 1)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
            else if (roll == 2)
            {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
            else if (roll == 3)
            {
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
            else if (roll == 4)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            }
        }
        checkCollision();
        if (grounded && upTouch && faceTouch && backTouch)
        {
            Debug.Log("IN WALL!!");
            return false;
        }
        else if (grounded == false && upTouch == false && faceTouch == false && backTouch == false)
        {
            Debug.Log("IN AIR!!");
            return false;
        }
        else
        {
            if (grounded)
            {
                Debug.Log("I think I'm grounded...");
                transform.rotation = Quaternion.identity;
            }
            if (faceTouch)
            {
                Debug.Log("I think my right side is making contact...");
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            if (backTouch)
            {
                Debug.Log("I think my left side is making contact...");
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            if (upTouch)
            {
                Debug.Log("I think my top side is making contact...");
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            return true;
        }
    }

    void checkCollision()
    {
        grounded = isGrounded();
        faceTouch = isTouchFace();
        backTouch = isTouchBack();
        upTouch = isTouchUp();
    }

    public override void Start()
    {
        origin = transform.position;
        //do nothing lol (yet...)
        int x;
        for (x = 0; x < 3 && moveToPosition() == false; x++)
        {

        }
        if (x >= 3)
        {
            Destroy(gameObject);
        }
        else
        {
            source.PlayOneShot(snareSound);
        }
    }

    public override void Update()
    {
        //do nothing lol (yet...)
        
        if (snaredObject)
        {
            if (snaredObject.nudging)
            {
                source.PlayOneShot(snareStruggle);
                Debug.Log("struggle");
                health = health - 1;
                snaredObject.nudging = false;
            }
        }

        if (health < 1 && destroyed == false)
        {
            snaredObject.stuck = false;
            destroyBullet();
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, col.bounds.extents.y + .3f, GameManager.instance.stageLayerMask);
        if (hit.collider)
        {
            Debug.DrawRay(transform.position, Vector2.down * (col.bounds.extents.y + .3f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * (col.bounds.extents.y + .3f), Color.red);
            return false;
        }
    }

    private bool isTouchFace()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, col.bounds.extents.y + .3f, GameManager.instance.stageLayerMask);
        if (hit.collider)
        {
            Debug.DrawRay(transform.position, Vector2.right * (col.bounds.extents.y + .3f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.right * (col.bounds.extents.y + .3f), Color.red);
            return false;
        }
    }

    private bool isTouchBack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, col.bounds.extents.y + .3f, GameManager.instance.stageLayerMask);
        if (hit.collider)
        {
            Debug.DrawRay(transform.position, Vector2.left * (col.bounds.extents.y + .3f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.left * (col.bounds.extents.y + .3f), Color.red);
            return false;
        }
    }

    private bool isTouchUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, col.bounds.extents.y + .3f, GameManager.instance.stageLayerMask);
        if (hit.collider)
        {
            Debug.DrawRay(transform.position, Vector2.up * (col.bounds.extents.y + .3f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.up * (col.bounds.extents.y + .3f), Color.red);
            return false;
        }
    }


    public override void onLevelContact()
    {
        //destroyBullet();
        //do nothing lol
    }

    public override void hitEnemyPlayer(PlayerMIRROR ply)
    {
        if (ply.stuck == false)
        {
            source.PlayOneShot(snareSound);
            Debug.Log("touching player");
            if (snaredObject == null)
            {
                ply.inflictSnare(gameObject);
                snaredObject = ply;
                anim.SetBool("caught", true);
            }
        } 
    }
}
