using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class WeaponPickup : Pickup
{
    Weapon thisWeapon;

    public override void CmdSetType(int wepSet)
    {
        thisWeapon = RainbowNetwork.instance.weaponList[wepSet];
        spriteRend.sprite = thisWeapon.pickupSprite;
    }

    private void Update()
    {
        if (spriteRend && thisWeapon)
        {
            if (spriteRend.sprite != thisWeapon.pickupSprite)
            {
                spriteRend.sprite = thisWeapon.pickupSprite;
            }
        } 
    }

    public override void CmdOnCollect(string player)
    {
        Debug.Log("GAGA");
        GameObject play = GameObject.Find(player);
        PlayerMIRROR collector = play.gameObject.GetComponent<PlayerMIRROR>();
        if (collector.hasWeapon(thisWeapon.id) == false)
        {
            if (hasAuthority)
            {
                collector.collectWeapon(thisWeapon.id);
            }
            removePickup(collector);
        }
    }
}
