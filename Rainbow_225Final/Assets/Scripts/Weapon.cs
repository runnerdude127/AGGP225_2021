using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string name;
    public GameObject bulletType;
    public bool auto;
    public int burst;
    public float delay;
    public float recoil;
    public float accuracy;
    public Sprite sprite;
    public AudioClip loadSound;
    public AudioClip shootSound;


    public Weapon(string newName, GameObject newBulletType, bool newAuto, int newBurst, float newDelay, float newRecoil, float newAccuracy, Sprite newSprite, AudioClip newLoadSound, AudioClip newShootSound)
    {
        name = newName;
        bulletType = newBulletType;
        auto = true;
        burst = newBurst;
        delay = newDelay;
        recoil = newRecoil;
        accuracy = newAccuracy;
        sprite = newSprite;
        shootSound = newShootSound;
        shootSound = newLoadSound;
    }
}
