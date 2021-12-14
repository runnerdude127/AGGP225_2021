using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    public GameObject equip;
    public bool destroyed = false;

    public float speed;
    public int damage;
    public float lifetime;

    Collider2D col;

    [HideInInspector]
    public Animator anim;
    public AnimationClip bulletType;
    public AnimationClip exitType;
    public GameObject creator;

    [HideInInspector]
    public AudioSource source;
    public AudioClip shotHit;
    public AudioClip enemyHit;

    public virtual void Awake()
    {
        col = gameObject.GetComponent<Collider2D>();
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        anim.Play(bulletType.name);
    }  
    
    public virtual void Start()
    {
        StartCoroutine(Decay());
    }

    public virtual void Update()
    {
        if (destroyed == false)
        {
            gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (creator)
        {
            if (collision.gameObject != creator && collision.tag != "Bullet")
            {
                Actor act = collision.gameObject.GetComponent<Actor>();
                if (act)
                {
                    //Debug.Log("actor found");
                    source.PlayOneShot(enemyHit);
                    Player ply = collision.gameObject.GetComponent<Player>();
                    if (ply)
                    {
                        if (ply.pv.IsMine)
                        {
                            Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                            ply.gameObject.GetPhotonView().RPC("playerDamage", RpcTarget.All, knockDir, damage, creator.GetPhotonView().ViewID);
                        }
                    }
                    else
                    {
                        Vector3 knockDir =  transform.position - collision.gameObject.transform.position;
                        StartCoroutine(act.Hurt(knockDir, damage, creator.GetPhotonView().ViewID));
                    }
                }
                else
                {
                    //Debug.Log("no actor found");
                }
                //Debug.Log("1 Trigger: " + collision.name + " Creator: " + creator.name);
                destroyBullet(false);
            }
            else if (collision.gameObject != creator && collision.tag != "Bullet")
            {
                //Debug.Log("2 Trigger: " + collision.name + " Creator: " + creator.name);
                destroyBullet(false);
            }
            else if (collision.tag == "Bullet")
            {
                Bullet bullethit = collision.gameObject.GetComponent<Bullet>();
                if (bullethit.creator != creator)
                {
                    destroyBullet(false);
                }
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (creator)
        {
            if (collision.gameObject != creator && collision.gameObject.tag != "Bullet")
            {
                Actor act = collision.gameObject.GetComponent<Actor>();
                if (act)
                {
                    source.PlayOneShot(enemyHit);
                    Player ply = collision.gameObject.GetComponent<Player>();
                    if (ply)
                    {
                        if (ply.pv.IsMine)
                        {
                            Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                            ply.gameObject.GetPhotonView().RPC("playerDamage", RpcTarget.All, knockDir, damage, creator.GetPhotonView().ViewID);
                        }
                    }
                    else
                    {
                        Vector3 knockDir = transform.position - collision.gameObject.transform.position;
                        StartCoroutine(act.Hurt(knockDir, damage, creator.GetPhotonView().ViewID));
                    }
                }
                else
                {
                    //Debug.Log("no actor found");
                }
                //Debug.Log("1 Collision: " + collision.gameObject.name + " Creator: " + creator.name);
                destroyBullet(false);
            }
            else if (collision.gameObject != creator && collision.gameObject.tag != "Bullet")
            {
                //Debug.Log("2 Collision: " + collision.gameObject.name + " Creator: " + creator.name);
                destroyBullet(false);
            }
            else if (collision.gameObject.tag == "Bullet")
            {
                Bullet bullethit = collision.gameObject.GetComponent<Bullet>();
                if (bullethit.creator != creator)
                {
                    destroyBullet(false);
                }
            }
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public virtual IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifetime);
        destroyBullet(true);
    }

    public virtual void destroyBullet(bool destroyedByDecay)
    {
        Destroy(col);
        destroyed = true;
        anim.Play(exitType.name);
        if (destroyedByDecay == false)
        {
            source.PlayOneShot(shotHit);
        }
        Destroy(gameObject, exitType.length);
    }
}
