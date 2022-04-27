using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : DefaultBullet
{
    bool secondPhase = false;
    public LayerMask floor;
    public GameObject toBlame;

    public AudioClip bounce;
    public override void Awake()
    {
        base.Awake();
        toBlame = PhotonManager.instance.gameObject;
    }

    public override void Update()
    {
        if (secondPhase && col)
        {
            isLanded();
        }
        else
        {
            base.Update();
        }
    }

    private bool isLanded()
    {
        if (col)
        {
            RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .2f, floor);
            if (hit.collider)
            {
                Debug.DrawRay(col.bounds.center, Vector2.down * (col.bounds.extents.y + .1f), Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(col.bounds.center, Vector2.down * (col.bounds.extents.y + .1f), Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }  
    }

    public override void onLevelContact()
    {
        if (secondPhase == false)
        {
            owner = GameManager.instance.LEVEL;
            ownedByPlayer = false;
            source.PlayOneShot(shotHit);
            rb.bodyType = RigidbodyType2D.Dynamic;
            col.isTrigger = false;
            rb.AddForce((speed / 4) * -transform.right, ForceMode2D.Impulse);
            rb.AddForce((speed / 3) * Vector2.up, ForceMode2D.Impulse);
            secondPhase = true;
            StartCoroutine(waitForBounces());
        }
        else
        {
            StartCoroutine(destroyDelay());
        }
    }

    IEnumerator waitForBounces()
    {
        yield return new WaitForSeconds(.05f);
        yield return new WaitUntil(() => isLanded());
        source.PlayOneShot(bounce);
        if (destroyed == false)
        {
            StartCoroutine(destroyDelay());
        }
    }

    IEnumerator destroyDelay()
    {
        destroyed = true;
        yield return new WaitForSeconds(1f);
        destroyBullet();
    }
}
