using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : DefaultBullet
{
    public GameObject splashEffect;

    public override void Start()
    {
        base.Start();
        rb.AddForce(speed * transform.right, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        if (rb)
        {
            transform.right = rb.velocity;
        }
    }

    public override void destroyBullet()
    {
        Instantiate(splashEffect, transform.position, Quaternion.identity);
        base.destroyBullet();
    }
}
