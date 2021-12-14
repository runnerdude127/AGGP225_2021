using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PlayerHand : PlayerPart
{
    public GameObject weapon;
    public GameObject player;

    Player myPlayer;

    public override void Start()
    {
        base.Start();
        myPlayer = player.GetComponent<Player>();
    }


    public override void Update()
    {
        /*if (mouse.x < transform.position.x)
        {
            spriteRend.flipX = true;
        }
        else
        {
            spriteRend.flipX = false;
        }*/
    }

    public IEnumerator Shoot()
    {
        MyWeapon myWeapon = weapon.GetComponent<MyWeapon>();
        PhotonView playerview = player.GetComponent<PhotonView>();
        PhotonView weaponview = myWeapon.GetComponent<PhotonView>();
        Debug.Log("two");
        myWeapon.Shoot(playerview.ViewID);
        Player applyForce = player.GetComponent<Player>();
        applyForce.ApplyRecoil(myWeapon.currentWeapon.recoil, -transform.right);
        myWeapon.cooldown = true;
        StartCoroutine(ShootAnim(myWeapon.currentWeapon.delay));
        yield return new WaitForSeconds(myWeapon.currentWeapon.delay);
        myWeapon.cooldown = false;
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator ShootAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void SpriteProcessing()
    {
        if (Input.GetKey(KeyCode.UpArrow) == true && Input.GetKey(KeyCode.DownArrow) == false)
        {
            anim.SetBool("LookUp", true);
            anim.SetBool("LookDown", false);
        }
        else if (Input.GetKey(KeyCode.UpArrow) == false && Input.GetKey(KeyCode.DownArrow) == true)
        {
            if (myPlayer.grounded == false)
            {
                anim.SetBool("LookUp", false);
                anim.SetBool("LookDown", true);
            }
        }
        else
        {
            anim.SetBool("LookUp", false);
            anim.SetBool("LookDown", false);
        }

        if (myPlayer.grounded == true && anim.GetBool("LookDown") == true)
        {
            anim.SetBool("LookDown", false);
        }
    }
}
