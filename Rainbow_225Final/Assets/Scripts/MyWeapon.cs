using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class MyWeapon : MonoBehaviour
{
    public Weapon currentWeapon;
    public GameObject barrel;

    public bool cooldown;
    public bool reloading;
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

    public IEnumerator Shoot(bool createdByPlayer, string creator)
    {
        GameObject maker = GameObject.Find(creator);
        PlayerMIRROR myPlayer = maker.GetComponent<PlayerMIRROR>();
        myPlayer.ApplyRecoil(currentWeapon.recoil, -transform.right);

        barrel.transform.localPosition = currentWeapon.barrelOffset;
        Vector3 shotPos = barrel.transform.position;

        cooldown = true;
        StartCoroutine(ShootAnim(currentWeapon.delay));

        if (currentWeapon.burst > 1)
        {
            for (int x = 0; x < currentWeapon.burst; x++)
            {
                //GameManager.instance.GetComponent<PhotonView>().RPC("makeShot", RpcTarget.All, currentWeapon.bulletID, createdByPlayer, creatorID, shotPos.x, shotPos.y, currentWeapon.accuracy, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                myPlayer.CmdMakeShot(currentWeapon.bulletID, createdByPlayer, creator, shotPos.x, shotPos.y, currentWeapon.accuracy, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }
        else
        {
            //GameManager.instance.GetComponent<PhotonView>().RPC("makeShot", RpcTarget.All, currentWeapon.bulletID, createdByPlayer, creatorID, shotPos.x, shotPos.y, currentWeapon.accuracy, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            myPlayer.CmdMakeShot(currentWeapon.bulletID, createdByPlayer, creator, shotPos.x, shotPos.y, currentWeapon.accuracy, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        source.PlayOneShot(currentWeapon.shootSound);
        

        StartCoroutine(reload());
        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator reload()
    {
        if (reloading == false)
        {
            reloading = true;
            yield return new WaitForSeconds(currentWeapon.delay);
            if (currentWeapon.delay > .5f)
            {
                source.PlayOneShot(currentWeapon.loadSound);
            }    
            cooldown = false;
            reloading = false;
        }
    }
}
