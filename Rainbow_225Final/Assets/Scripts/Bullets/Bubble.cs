using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : DefaultBullet
{
    public GameObject splashEffect;

    public override void Start()
    {
        base.Start();
        rb.AddForce(speed * transform.right, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        // dont do anything lol
    }

    public override void destroyBullet()
    {
        Instantiate(splashEffect, transform.position, Quaternion.identity);
        base.destroyBullet();
    }
}
