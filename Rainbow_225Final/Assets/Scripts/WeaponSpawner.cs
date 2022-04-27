using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class WeaponSpawner : Spawner
{
    public int weaponID = 0;

    [Server]
    public override void CmdSpawnObject()
    {
        Debug.Log("WEAPON spawn");
        GameObject newSpawn = Instantiate(spawnedObject, mySpawnPoint.transform.position, Quaternion.identity);
        WeaponPickup wep = newSpawn.GetComponent<WeaponPickup>();
        if (wep)
        {
            if (weaponID == -1)
            {
                wep.CmdSetType(Random.Range(0, RainbowNetwork.instance.weaponList.Count));
            }
            else
            {
                wep.CmdSetType(weaponID);
            }
        }
        NetworkServer.Spawn(newSpawn);
        mySpawns.Add(newSpawn);
    }

}
