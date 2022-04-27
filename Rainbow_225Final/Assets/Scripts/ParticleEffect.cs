using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : Effect
{
    ParticleSystem particles;

    public override void Awake()
    {
        base.Awake();
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
        duration = particles.main.duration + particles.main.startLifetimeMultiplier;
    }
}
