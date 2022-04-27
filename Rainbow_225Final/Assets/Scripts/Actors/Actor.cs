using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Actor : NetworkBehaviour
{
    public bool isDamageable;
    public bool hasHealth = false;
    public bool respawns = false;
    public bool isRespawning = false;

    bool isFlashing = false;
    bool isBlinking = false;
    bool isShaking = true;
    public bool isEnemy;
    
    public bool doKnockback = true;
    public GameObject deathEffect;
    Vector2 spawnOrigin;


    public float invulnTime = .5f;

    #region statuses

    public bool dead = false;
    public bool invulnerable = false;
    public bool stunned = false;
    public bool flung = false;
    public bool stuck = false;

    public bool burning = false;
    public bool frozen = false;
    public bool poisoned = false;

    #endregion

    public bool nudging = false;

    public bool grounded;
    public bool faceTouch;
    public bool backTouch;
    public bool upTouch;

    public int currentHealth;
    public int health = 10;

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

    public GameObject respawnEffect;
    public GameObject spawnEffect;

    public AudioClip damageSound;
    public AudioClip mortalDamage;
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

    public virtual void Update()
    {
        grounded = isGrounded();
        faceTouch = isTouchFace();
        backTouch = isTouchBack();
        upTouch = isTouchUp();

        if (damageRacked > 0 && checking == false)
        {
            StartCoroutine(damageNumberCheck());
        }

        if (dead == true && grounded == true)
        {
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //rb.velocity = rb.velocity / 2;
        }

        checkBlinks();
    }

    public void checkBlinks()
    {
        if (isFlashing == false && invulnerable)
        {
            StartCoroutine(invulnFlash());
        }

        if (isBlinking == false && stunned)
        {
            StartCoroutine(stunBlink());
        }

        if (isShaking == false && stunned)
        {
            StartCoroutine(stunShake());
        }
    }

    public void OnDrawGizmos()
    {
        if (col)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            /*Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector2(col.bounds.center.x, col.bounds.center.y - .2f), new Vector2(col.bounds.size.x + .15f, col.bounds.size.y));
            Gizmos.DrawWireCube(new Vector2(col.bounds.center.x, col.bounds.center.y + .2f), new Vector2(col.bounds.size.x + .15f, col.bounds.size.y));
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(new Vector2(col.bounds.center.x - .2f, col.bounds.center.y), new Vector2(col.bounds.size.x, col.bounds.size.y + .15f));
            Gizmos.DrawWireCube(new Vector2(col.bounds.center.x + .2f, col.bounds.center.y), new Vector2(col.bounds.size.x, col.bounds.size.y + .15f));*/
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .2f, GameManager.instance.stageLayerMask.value);
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

    private bool isTouchFace()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, new Vector2(col.bounds.size.x, col.bounds.size.y / 1.5f), 0f, gameObject.transform.right, .2f, GameManager.instance.stageLayerMask.value);
        if (hit.collider)
        {
            Debug.DrawRay(col.bounds.center, gameObject.transform.right * (col.bounds.extents.x + .1f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(col.bounds.center, gameObject.transform.right * (col.bounds.extents.x + .1f), Color.red);
            return false;
        }
    }

    private bool isTouchBack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, new Vector2(col.bounds.size.x, col.bounds.size.y / 1.5f), 0f, -gameObject.transform.right, .2f, GameManager.instance.stageLayerMask.value);
        if (hit.collider)
        {
            Debug.DrawRay(col.bounds.center, -gameObject.transform.right * (col.bounds.extents.x + .1f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(col.bounds.center, -gameObject.transform.right * (col.bounds.extents.x + .1f), Color.red);
            return false;
        }
    }

    private bool isTouchUp()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.up, .2f, GameManager.instance.stageLayerMask.value);
        if (hit.collider)
        {
            Debug.DrawRay(col.bounds.center, Vector2.up * (col.bounds.extents.y + .1f), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(col.bounds.center, Vector2.up * (col.bounds.extents.y + .1f), Color.red);
            return false;
        }
    }

    public virtual IEnumerator Hurt(Vector3 knockDir, int damage, string attack, float stunTime)
    {
        if (dead == false && invulnerable == false)
        {
            damageRacked += damage;
            string deathCause;
            GameObject attacker = GameObject.Find(attack);

            if (attacker)
            {
                deathCause = attacker.name;
            }
            else
            {
                deathCause = "natural causes";
            }

            source.PlayOneShot(damageSound);

            StartCoroutine(stunFrames(stunTime));
            //StartCoroutine(invulnFrames(stunTime));

            //Debug.Log(gameObject.name + " has been hurt for " + damage + " damage!");

            yield return new WaitUntil(() => stunned == false);
            if (doKnockback == true && rb)
            {
                rb.AddForce(-knockDir * damage / 2, ForceMode2D.Impulse);
            }
            if (hasHealth)
            {
                currentHealth -= damage;
                if (currentHealth <= 0)
                {
                    PlayerMIRROR me = gameObject.GetComponent<PlayerMIRROR>();
                    if (me)
                    {
                        //me.pv.RPC("playerDies", RpcTarget.All, deathCause, knockDir.x, knockDir.y, knockDir.z);
                    }
                    else
                    {
                        StartCoroutine(actorDies(deathCause, knockDir));
                    }
                }
            }
        }
    }

    public IEnumerator damageNumberCheck()
    {
        if (damageRacked > 0)
        {
            checking = true;
            if (invulnerable == true)
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

    public IEnumerator actorDies(string cause, Vector3 knockDir)
    {
        if (damageRacked > 0)
        {
            GameObject number = Instantiate(damageNumber, transform.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0), Quaternion.identity);
            DamageNumber damNum = number.GetComponent<DamageNumber>();
            damNum.damageAmount = damageRacked;
            damageRacked = 0;
        } 
        if (dead == false)
        {
            dead = true;

            rb.constraints = RigidbodyConstraints2D.None;
            //rb.velocity = new Vector2(0, 0);

            source.PlayOneShot(mortalDamage);
            //Debug.Log("knockdir : " + knockDir);
            rb.AddForce(transform.up * 5, ForceMode2D.Impulse);
            rb.AddForce(-knockDir * 10, ForceMode2D.Impulse);
            rb.AddTorque(rb.velocity.x * 5);
            Debug.Log(this.gameObject.name + " was killed by " + cause);

            yield return new WaitForSeconds(5f);
            if (respawns)
            {
                StartCoroutine(Respawn(false));
            }
            else
            {
                rb.isKinematic = true;
            }
        } 
    }

    public virtual IEnumerator Respawn(bool firstSpawn)
    {
        isRespawning = true;
        spriteRend.material.SetInt("Boolean_E47E2B67", 1);
        spriteRend.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (firstSpawn == false)
        {
            Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);

            // fade out
        }
        rb.velocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = getSpawnPoint();
        currentHealth = health;
        spriteRend.enabled = true;

        // fade in

        // cool animation
        if (respawnEffect)
        {
            Instantiate(respawnEffect, new Vector3(transform.position.x, transform.position.y, -9), Quaternion.identity);
            spriteRend.material.SetColor("Color_D7188EF4", Color.white);
        }
        yield return new WaitForSeconds(respawnEffect.GetComponent<Effect>().effectSound.length + .5f);
        if (spawnEffect)
        {
            Instantiate(spawnEffect, new Vector3(transform.position.x, transform.position.y, -9), Quaternion.identity);
            spriteRend.material.SetColor("Color_D7188EF4", Color.black);
        }
        yield return new WaitForSeconds(.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;
        invulnerable = false;
        dead = false;
        isRespawning = false;
    }

    public virtual Vector2 getSpawnPoint()
    {
        return spawnOrigin;
    }

    public IEnumerator stunFrames(float stun)
    {
        stunned = true;
        if (stun >= .05f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        yield return new WaitForSeconds(stun);
        if (stun >= .05f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        stunned = false;
    }

    public IEnumerator invulnFrames(float stun)
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnTime + stun);
        invulnerable = false;
    }

    IEnumerator stunBlink()
    {
        isBlinking = true;
        while (stunned)
        {
            spriteRend.material.SetColor("Color_D7188EF4", Color.white);
            yield return new WaitForSeconds(0.05f);
            spriteRend.material.SetColor("Color_D7188EF4", Color.black);
            yield return new WaitForSeconds(0.05f);
        }
        isBlinking = false;
    }

    IEnumerator stunShake()
    {
        isShaking = true;

        Vector3 origin = transform.localPosition;
        float power = .05f;

        while (stunned)
        {
            float x = Random.Range(-1f, 1f) * power;
            float y = Random.Range(-1f, 1f) * power;

            transform.localPosition = new Vector3(origin.x + x, origin.y + y, origin.z);
            yield return null;
        }

        transform.localPosition = origin;
        isShaking = false;
    }

    IEnumerator invulnFlash()
    {
        isFlashing = true;
        while(invulnerable)
        {
            spriteRend.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRend.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false;
    }

    #region status inflictions

    public virtual void inflictSnare(GameObject gameObject)
    {
        // get stuck

        stuck = true;
        //transform.position = gameObject.transform.position;
    }

    #endregion
}
