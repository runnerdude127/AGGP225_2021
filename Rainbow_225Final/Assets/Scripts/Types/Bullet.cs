using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet")]
public class Bullet : ScriptableObject
{
    public GameObject thisPrefab;
    public new string name;
    public Sprite icon;
    public float speed;
    public int damage;
    public float hitStun;
    public float lifetime;
    public AnimationClip bulletType;
    public AnimationClip exitType;
    public AudioClip shotHit;
    public AudioClip enemyHit;

    public Bullet(GameObject newPrefab)
    {
        thisPrefab = newPrefab;
        DefaultBullet newBulletData = thisPrefab.GetComponent<DefaultBullet>();
        if (newBulletData)
        {
            name = newBulletData.name;
            icon = newBulletData.rend.sprite;
            speed = newBulletData.speed;
            damage = newBulletData.damage;
            hitStun = newBulletData.hitStun;
            lifetime = newBulletData.lifetime;
            bulletType = newBulletData.bulletType;
            exitType = newBulletData.exitType;
            shotHit = newBulletData.shotHit;
            enemyHit = newBulletData.enemyHit;
        }
    }
}
