using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPart : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Animator anim;
    public bool fullRotate;
    public bool normalFlip;

    public virtual void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }


    public virtual void Update()
    {
            /*if (normalFlip)
            {
                Vector3 flip = transform.localScale;
                flip.x = 1;
                transform.localScale = flip;
                if (spriteRend)
                {
                    spriteRend.flipX = false;
                }
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (spriteRend)
                {
                    spriteRend.flipX = false;
                }
            }*/
    }


    public void SetAnim(string animation)
    {
        anim.Play(animation);
    }

}
