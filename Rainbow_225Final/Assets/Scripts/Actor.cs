using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Actor : MonoBehaviour
{
    public bool isDamageable;
    public bool hasHealth = false;
    public bool respawns = false;
    public float invulnTime = .001f;
    public bool isEnemy;
    public bool damageShake = true;
    public bool doKnockback = true;
    public GameObject deathEffect;
    Vector2 spawnOrigin;
    bool isDead = false;

    public int currentHealth;
    public int health = 10;

    [HideInInspector]
    public bool invulnurable = false;
    [HideInInspector]
    public bool blinkCooldown = false;

    [HideInInspector]
    public SpriteRenderer spriteRend;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public float basePitch;

    public AudioClip damageSound;
    public GameObject damageNumber;
    public int damageRacked = 0;
    public bool checking = false;

    public virtual void Awake()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();
        source = gameObject.GetComponent<AudioSource>();

        spawnOrigin = transform.position;
        rb.isKinematic = false;
    }

    public virtual void Start()
    {
        basePitch = source.pitch;

        currentHealth = health;
    }

    void Update()
    {
        if (damageRacked > 0 && checking == false)
        {
            StartCoroutine(damageNumberCheck());
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDamageable == true)
        {
            if (collision.tag == "Bullet" && invulnurable == false)
            {
                Bullet hitBy = collision.gameObject.GetComponent<Bullet>();
                if (hitBy.creator != this.gameObject)
                {
                    Vector3 knockDir = collision.gameObject.transform.position - transform.position;
                    int damageAmount = hitBy.GetDamage();
                    StartCoroutine(Hurt(knockDir, damageAmount));
                }
            }
        }
    }*/

    public virtual IEnumerator Hurt(Vector3 knockDir, int damage, int attacker)
    {
        invulnurable = true;
        damageRacked += damage;
        string deathCause;
        PhotonView attackView = PhotonView.Find(attacker);

        if (attackView)
        {
            GameObject realAttacker = attackView.gameObject;
            deathCause = realAttacker.name;
        }
        else
        {
            deathCause = "natural causes";
        }

        source.PlayOneShot(damageSound);
        StartCoroutine(InvulnFlash());
        //Debug.Log(gameObject.name + " has been hurt for " + damage + " damage!");
        if (doKnockback == true && rb)
        {
            rb.AddForce(-knockDir * damage / 2, ForceMode2D.Impulse);
        }
        if (hasHealth)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Player me = gameObject.GetComponent<Player>();
                if (me)
                {
                    me.pv.RPC("playerDies", RpcTarget.All, deathCause);
                }
                else
                {
                    StartCoroutine(actorDies(deathCause));
                }
            }
        }
        yield return new WaitForSeconds(invulnTime);
        invulnurable = false;
    }

    public IEnumerator damageNumberCheck()
    {
        if (damageRacked > 0)
        {
            checking = true;
            if (invulnurable == true || blinkCooldown == true)
            {
                
            }
            else
            {
                GameObject number = Instantiate(damageNumber, transform.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0), Quaternion.identity);
                DamageNumber damNum = number.GetComponent<DamageNumber>();
                damNum.damageAmount = damageRacked;
                damageRacked = 0;
                yield return new WaitForSeconds(invulnTime);
            }
            yield return new WaitForSeconds(invulnTime * 2);
            checking = false;
        }
    }


    public IEnumerator actorDies(string cause)
    {
        if (damageRacked > 0)
        {
            GameObject number = Instantiate(damageNumber, transform.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0), Quaternion.identity);
            DamageNumber damNum = number.GetComponent<DamageNumber>();
            damNum.damageAmount = damageRacked;
            damageRacked = 0;
        } 
        if (isDead == false)
        {
            isDead = true;

            spriteRend.enabled = false;
            col.enabled = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = new Vector2(0, 0);

            Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
            rb.AddForce(new Vector3((Random.Range(-1f, 1f) * 10), (Random.Range(-1f, 1f) * 10), (Random.Range(-1f, 1f) * 10)), ForceMode2D.Impulse);
            Debug.Log(this.gameObject.name + " was killed by " + cause);

            yield return new WaitForSeconds(2f);
            if (respawns)
            {
                rb.velocity = new Vector2(0, 0);
                transform.position = getSpawnPoint();
                currentHealth = health;

                spriteRend.enabled = true;
                col.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                isDead = false;
            }
            else
            {
                rb.isKinematic = true;
            }
        } 
    }

    public virtual Vector2 getSpawnPoint()
    {
        return spawnOrigin;
    }

    IEnumerator InvulnFlash()
    {
        while (invulnurable == true && blinkCooldown == false)
        {
            if (damageShake == true)
            {
                StartCoroutine(DamageShake(.1f, .05f));
            }
            blinkCooldown = true;
            spriteRend.material.color = new Color(45, 200, 20, 1);
            yield return new WaitForSeconds(0.05f);
            spriteRend.material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            blinkCooldown = false;
        }
    }

    IEnumerator DamageShake(float time, float power)
    {
        Vector3 origin = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * power;
            float y = Random.Range(-1f, 1f) * power;

            transform.localPosition = new Vector3(origin.x + x, origin.y + y, origin.z);
            yield return null;
            elapsed += Time.deltaTime;
        }
        transform.localPosition = origin;
    }
}
