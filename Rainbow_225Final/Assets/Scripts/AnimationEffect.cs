using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : Effect
{
    Animator anim;
    SpriteRenderer rend;
    public AnimationClip effectAnimation;

    public override void Awake()
    {
        base.Awake();
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (effectSound.length < effectAnimation.length)
        {
            duration = effectAnimation.length;
            
        }
        else
        {
            duration = effectSound.length;
            Destroy(rend, effectAnimation.length);
        } 
    }
}
