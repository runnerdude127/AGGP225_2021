using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Pickup : NetworkBehaviour
{
    public bool magnetic = false;
    public bool decays = false;
    public float lifeTime = 8;
    public float decayTime = 8;

    public float floatAmplitude = .2f;
    public float floatSpeed = 2f;

    AudioSource source;
    public AudioClip collectSound;
    public SpriteRenderer spriteRend;
    ParticleSystem particles;
    bool alive = true;
    Vector3 origin;

    public virtual void Awake()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        source = gameObject.GetComponent<AudioSource>();
        particles = gameObject.GetComponent<ParticleSystem>();
        origin = transform.position;
    }

    void FixedUpdate()
    {
        if (alive)
        {
            transform.position = new Vector3(origin.x, origin.y + (floatAmplitude * Mathf.Sin(floatSpeed * Time.time)));
        }
    }

    public virtual void CmdSetType(int id)
    {
        // null
    }

    /*Vector3 destination = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, 0);
                Vector3 smooth = Vector3.Lerp(transform.position, destination, .125f);
                transform.position = smooth;*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMIRROR ply = collision.gameObject.GetComponent<PlayerMIRROR>();
        if (ply && alive)
        {
            CmdOnCollect(ply.gameObject.name);
        }
    }

    public virtual void CmdOnCollect(string player)
    {
        Debug.Log("GAGA");
        GameObject play = GameObject.Find(player);
        PlayerMIRROR collector = play.gameObject.GetComponent<PlayerMIRROR>();
        removePickup(collector);
    }

    public void removePickup(PlayerMIRROR collector)
    {
        alive = false;
        Debug.Log(collector.gameObject.name + "collected the " + gameObject.name);
        source.PlayOneShot(collectSound);
        Destroy(spriteRend);
        Destroy(particles);
        Destroy(gameObject, collectSound.length);
    }
}
