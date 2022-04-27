using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : DefaultBullet
{
    int turns = 0;
    public float dist = .2f;
    public LayerMask floor;

    public AudioClip turn;

    public override void Update()
    {
        if (isTouchingWall())
        {
            switchDir();
        }
        if (rb)
        {
            base.Update();
        }
        
        
    }

    void switchDir()
    {
        turns++;
        if (turns > 3 && destroyed == false)
        {
            properDestroy();
        }
        else
        {
            if (isAboveLand())
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            gameObject.transform.Translate(0, -speed / 2 * Time.deltaTime, 0);
            source.PlayOneShot(turn);
        }  
    }

    public override void onLevelContact()
    {
        if (rb && destroyed == false)
        {
            gameObject.transform.Translate(-speed / 2 * Time.deltaTime, 0, 0);
            switchDir();
        }
    }

    private bool isAboveLand()
    {
        if (col)
        {
            RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, -transform.up, dist, floor);
            if (hit.collider)
            {
                Debug.DrawRay(col.bounds.center, -transform.up * (col.bounds.extents.y + dist), Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(col.bounds.center, -transform.up * (col.bounds.extents.y + dist), Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool isTouchingWall()
    {
        if (col)
        {
            RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, transform.right, dist, floor);
            if (hit.collider)
            {
                Debug.DrawRay(col.bounds.center, transform.right * (col.bounds.extents.y + dist), Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(col.bounds.center, transform.right * (col.bounds.extents.y + dist), Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void properDestroy()
    {
        destroyed = true;
        gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
        source.PlayOneShot(enemyHit);
        rb.velocity = Vector2.zero;
        Destroy(rb);
        Destroy(col);
        anim.Play(exitType.name);
        Destroy(gameObject, exitType.length);
    }
}
