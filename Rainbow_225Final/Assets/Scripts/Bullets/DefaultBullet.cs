using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class DefaultBullet : NetworkBehaviour
{
    public bool destroyed = false;

    public float speed;
    public int damage;
    public float hitStun;
    public float lifetime;

    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public SpriteRenderer rend;

    [HideInInspector]
    public Animator anim;
    public AnimationClip bulletType;
    public AnimationClip exitType;

    public bool ownedByPlayer;
    public string owner = null;

    [HideInInspector]
    public AudioSource source;
    public AudioClip shotHit;
    public AudioClip enemyHit;

    public virtual void Awake()
    {
        col = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        anim.Play(bulletType.name);
    }  
    
    public virtual void Start()
    {
        StartCoroutine(bulletDecay());
    }

    public virtual void Update()
    {
        if (destroyed == false)
        {
            gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    public virtual IEnumerator bulletDecay()
    {
        yield return new WaitForSeconds(lifetime);
        destroyBullet();
    }

    // Has both trigger and collision enters for maximum adaptability. they both call the "getContact" function
    #region collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        getContact(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        getContact(collision.gameObject);
    }

    void getContact(GameObject contact)
    {
        if (destroyed == false) // bullet isn't destroyed
        {
            Actor act = contact.GetComponent<Actor>();
            DefaultBullet bul = contact.GetComponent<DefaultBullet>();
            if (act) // hit some kind of actor
            {
                PlayerMIRROR ply = contact.GetComponent<PlayerMIRROR>();
                if (ply)    // hit a player
                {
                    onPlayerContact(ply);
                }
                else   // hit an actor
                {
                    onActorContact(act);
                }
            }
            else if (bul)   // hit a bullet
            {
                onBulletContact(bul);
            }
            else if ((GameManager.instance.stageLayerMask.value & (1 << contact.layer)) > 0)  // hit the stage
            {
                onLevelContact();
                
            }
            else   // any other collision
            {
                Debug.LogWarning(gameObject.name + " has touched an unknown element! Touched: " + contact.name);
            }
        }
    }

    #endregion

    // Handling for when this bullet touches another gameObject. They can be overridden!
    #region contacts

    public virtual void onActorContact(Actor act)
    {
        Debug.Log(gameObject.name + "has touched Actor: " + act.gameObject.name);
        source.PlayOneShot(enemyHit);
        Vector3 knockDir = transform.position - act.gameObject.transform.position;
        StartCoroutine(act.Hurt(knockDir, damage, owner, hitStun));
        destroyBullet();
    }

    public virtual void onPlayerContact(PlayerMIRROR ply)
    {
        Debug.Log(gameObject.name + "has touched Player: " + ply.gameObject.name);
        GameObject maker = GameObject.Find(owner);
        GameObject playerHit = ply.gameObject;
        if (maker)  // the bullet has a proper creator
        {
            NetworkIdentity makerOnline = maker.GetComponent<NetworkIdentity>();
            NetworkIdentity hitOnline = playerHit.GetComponent<NetworkIdentity>();

            if (makerOnline && hitOnline)   // the creator of this bullet and what it hit are online players
            {
                if (maker != playerHit) // the player hit is not the one who created this bullet
                {
                    GameObject myCreator = makerOnline.gameObject;
                    PlayerMIRROR myPlayer = myCreator.GetComponent<PlayerMIRROR>();
                    bool teams = false;
                    if (myPlayer.myTeam != null)
                    {
                        teams = true;
                    }

                    if (teams)  // the players have teams (TEAM MATCH)
                    {
                        if (myPlayer.myTeam != ply.myTeam)  // the teams are enemies
                        {
                            if (ply.invulnerable)  // player is invulnerable
                            {
                                destroyBullet();
                            }
                            else  // player isn't invulnerable
                            {
                                hitEnemyPlayer(ply);
                            }
                        }
                        else // the teams are not enemies
                        {
                            hitAllyPlayer(ply);
                        }
                    }
                    else   // the players don't have teams (FFA)
                    {
                        if (ply.invulnerable)  // player is invulnerable
                        {
                            destroyBullet();
                        }
                        else  // player isn't invulnerable
                        {
                            hitEnemyPlayer(ply);
                        }
                    }
                }
                else // the player hit is the one who created this bullet
                {

                }
            }

            if (makerOnline == false) // if the player who made this isn't online
            {
                Debug.LogError("The player who created this bullet has no PhotonView.");
                destroyBullet();
            }
            if (makerOnline == false) // if the player who was hit isn't online
            {
                Debug.LogError("The player hit has no PhotonView.");
                destroyBullet();
            }
        }
        else  // the bullet's owner is an outside source (has a world ID attached)
        {
            hitEnemyPlayer(ply);
        }

        


        /*if (ply.pv.IsMine)
        {

        }*/
    }

    public virtual void onBulletContact(DefaultBullet bul)
    {
        Debug.Log(gameObject.name + "has touched Bullet: " + bul.gameObject.name);

        if (bul.owner != owner) // the bullets do not have the same creator
        {
            bul.hitEnemyBullet(bul);
            if (bul.damage >= damage) // the enemy bullet has more damage than this bullet
            {
                destroyBullet();
            }
        }
    }

    public virtual void onLevelContact()
    {
        destroyBullet();
    }

    #endregion

    // Handling for when this bullet HITS another gameObject. These are more specific. They can be overridden!
    #region reactions

    public virtual void destroyBullet()    // Destroy the bullet
    {
        destroyed = true;
        source.PlayOneShot(shotHit);
        rend.sortingLayerName = "FallOff";
        Destroy(rb);
        Destroy(col);
        anim.Play(exitType.name);
        Destroy(gameObject, exitType.length);
    }

    public virtual void hitEnemyPlayer(PlayerMIRROR ply)
    {
        Debug.Log(gameObject.name + " has hit an enemy: " + ply.gameObject.name);
        source.PlayOneShot(enemyHit);
        Vector3 knockDir = transform.position - ply.gameObject.transform.position;
        ply.CmdDoPlayerDamage(knockDir, damage, owner, hitStun);
        destroyBullet();
    }

    public virtual void hitAllyPlayer(PlayerMIRROR ply)
    {
        Debug.Log(gameObject.name + " has hit an ally: " + ply.gameObject.name);
        // do something relating to allies
    }

    public virtual void hitReflector(string reflector, bool isPlayerReflector)
    {
        rb.velocity = -rb.velocity;
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.x + 180);
        Debug.Log(gameObject.name + " was reflected by: " + reflector);
        owner = reflector;
        ownedByPlayer = isPlayerReflector;
    }

    public virtual void hitEnemyBullet(DefaultBullet bul)
    {
        int opposing = bul.damage;
        if (damage > opposing)
        {
            damage = damage - 1;
            if (damage < 1)
            {
                destroyBullet();
            }
        }
        else
        {
            destroyBullet();
        }
    }

    #endregion
}
