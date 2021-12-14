using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class MyWeapon : MonoBehaviour
{
    public static MyWeapon instance;
    public GameObject weapon;
    public GameObject weaponManager;

    public bool isWeapon;
    public bool facesMouse;
    public Weapon currentWeapon;

    public bool cooldown;

    SpriteRenderer spriteRend;
    Animator anim;
    AudioSource source;

    public AudioClip shootSound;

    void Awake()
    {
        if (isWeapon)
        {
            instance = this;
        }
    }

    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isWeapon)
        {
            WeaponManager getStats = weaponManager.GetComponent<WeaponManager>();
            currentWeapon = getStats.GetWeapon();
            spriteRend.sprite = currentWeapon.sprite;
        }
    }


    public void Shoot(int player)
    {
        Debug.Log("three");
        PhotonView playerMaker = PhotonView.Find(player);
        PhotonView weaponView = gameObject.GetComponent<PhotonView>();
        GameObject maker = playerMaker.gameObject;

        if (weaponView.IsMine)
        {
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
        } 
    }
}
