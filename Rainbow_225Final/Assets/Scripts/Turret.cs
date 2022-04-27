using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Actor
{
    public int weaponID = 0;
    int weaponMemory;
    Weapon currentWeapon;

    public override void Start()
    {
        currentWeapon = PhotonManager.instance.GetWeapon(weaponID);
        Debug.Log(currentWeapon.name);
        StartCoroutine(TurretShootCycle());

        weaponMemory = weaponID;
    }

    public override void Update()
    {
        base.Update();
        if (weaponID != weaponMemory)
        {
            currentWeapon = PhotonManager.instance.GetWeapon(weaponID);
        }  
    }

    IEnumerator TurretShootCycle()
    {
        for (int x = 0; x < currentWeapon.burst; x++)
        {
            GameObject shot = Instantiate(PhotonManager.instance.GetBullet(currentWeapon.bulletID).thisPrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-currentWeapon.accuracy, currentWeapon.accuracy)));
            DefaultBullet thisShot = shot.GetComponent<DefaultBullet>();
            thisShot.owner = gameObject.name;
        }

        if (currentWeapon.auto == false)
        {
            yield return new WaitForSeconds(currentWeapon.delay + 1f);
        }
        else
        {
            yield return new WaitForSeconds(currentWeapon.delay);
        }
        StartCoroutine(TurretShootCycle());
    }
}
