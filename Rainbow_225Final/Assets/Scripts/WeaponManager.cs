using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weaponList;
    public int currentWeapon;

    AudioSource source;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeapon++;
            if (currentWeapon == weaponList.Count)
            {
                currentWeapon = 0;
            }
            source.PlayOneShot(weaponList[currentWeapon].loadSound);
        }
    }

    public Weapon GetWeapon()
    {
        return weaponList[currentWeapon];
    }
}
