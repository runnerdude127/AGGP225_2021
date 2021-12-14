using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class MyWeapon : MonoBehaviour
{
    //public bool facesMouse;
    public Weapon currentWeapon;

    public bool cooldown;
    public bool isWeapon;

    public SpriteRenderer spriteRend;
    public Animator anim;

    AudioSource source;
    public AudioClip shootSound;

    void Awake()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    IEnumerator ShootAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public IEnumerator Shoot(int player)
    {
        PhotonView playerMaker = PhotonView.Find(player);
        GameObject maker = playerMaker.gameObject;

        cooldown = true;
        StartCoroutine(ShootAnim(currentWeapon.delay));

        if (currentWeapon.burst > 1)
        {
            for (int x = 0; x < currentWeapon.burst; x++)
            {
                GameObject shot = Instantiate(currentWeapon.bulletType, transform.position, transform.parent.rotation * Quaternion.Euler(0, 0, Random.Range(-currentWeapon.accuracy, currentWeapon.accuracy)));
                Bullet thisShot = shot.GetComponent<Bullet>();
                if (maker)
                {
                    thisShot.creator = maker;
                }
            }
        }
        else
        {
            GameObject shot = Instantiate(currentWeapon.bulletType, transform.position, transform.parent.rotation * Quaternion.Euler(0, 0, Random.Range(-currentWeapon.accuracy, currentWeapon.accuracy)));
            Bullet thisShot = shot.GetComponent<Bullet>();
            if (maker)
            {
                thisShot.creator = maker;
            }
        }
        source.PlayOneShot(currentWeapon.shootSound);

        yield return new WaitForSeconds(currentWeapon.delay);
        cooldown = false;
        yield return new WaitForSeconds(0.05f);
    }
}
