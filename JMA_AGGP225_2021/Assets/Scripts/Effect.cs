using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem ps;

    AudioSource source;
    public AudioClip effectSound;

    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        source = gameObject.GetComponent<AudioSource>();

        source.PlayOneShot(effectSound);
        detonate();
    }

    void detonate()
    {
        if (source && effectSound)
        {
            if (ps.duration > effectSound.length)
            {
                Destroy(gameObject, ps.duration);
            }
            else
            {
                Destroy(gameObject, effectSound.length);
            }
        }
        else
        {
            Destroy(gameObject, effectSound.length);
        }
    }
}
