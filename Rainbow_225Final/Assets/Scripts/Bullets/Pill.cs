using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : DefaultBullet
{
    int bounces = 0;

    public override void Start()
    {
        base.Start();
        rb.AddForce(speed / 2 * transform.right, ForceMode2D.Impulse);
        rb.AddForce(speed * transform.up, ForceMode2D.Impulse);
        StartCoroutine(turnOffTrigger());
    }

    public override void Update()
    {
        if (rb && destroyed == false)
        {
            transform.right = rb.velocity;
        }
    }

    public override void onLevelContact()
    {
        if (bounces < 3)
        {
            bounces++;
            source.PlayOneShot(shotHit);
        }
        else
        {
            destroyBullet();
        }
    }

    IEnumerator turnOffTrigger()
    {
        yield return new WaitForSeconds(.1f);
        col.isTrigger = false;
    }

    /*void properDestroy()
    {
        destroyed = true;
        rb.velocity = Vector2.zero;
        transform.right = rb.velocity;
        source.PlayOneShot(enemyHit);
        Destroy(rb);
        Destroy(col);
        anim.Play(exitType.name);
        Destroy(gameObject, exitType.length);
    }*/
}
