using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public bool collides = false;
    public float duration;

    Collider2D col;
    AudioSource source;
    public AudioClip effectSound;

    public virtual void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        if (collides)
        {
            col = gameObject.GetComponent<Collider2D>();
        }
    }

    public virtual void Start()
    {
        if (effectSound)
        {
            source.PlayOneShot(effectSound);
        }
        if (col)
        {
            Destroy(col, duration / 4);
        }
        Destroy(gameObject, duration);
    }
}
