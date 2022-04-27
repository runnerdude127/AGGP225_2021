using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Spawner : NetworkBehaviour
{
    public GameObject spawnedObject;
    public int maxSpawned;
    public bool exactSpawnTime = true;
    public float spawnTime = 5f;
    public float randomWiggleRoom = 0;
    public GameObject mySpawnPoint;
    public int spawnerID = -1;
    int pickupID;

    public List<GameObject> mySpawns;
    public bool checkingSpawns = false;
    public bool spawnCycleOn = false;

    private void Awake()
    {
        Debug.Log("ACTIVE");
    }

    private void Update()
    {
        if (mySpawns.Count >= maxSpawned)
        {
            if (checkingSpawns == false)
            {
                checkMySpawns();
            }
        }
        else if (mySpawns.Count < maxSpawned)
        {
            if (spawnCycleOn == false)
            {
                Debug.Log("SPAWNCYCLE");
                StartCoroutine(SpawnCycle());
            }
        }
    }

    IEnumerator SpawnCycle()
    {
        spawnCycleOn = true;

        if (exactSpawnTime)
        {
            yield return new WaitForSeconds(spawnTime);
        }
        else
        {
            float randomTime = spawnTime + Random.Range(-randomWiggleRoom, randomWiggleRoom);
            if (randomTime < 0.01)
            {
                randomTime = 0.01f;
            }
            yield return new WaitForSeconds(randomTime);
        }
        Debug.Log("spawn!");
        CmdSpawnObject();

        if (mySpawns.Count < maxSpawned)
        {
            Debug.Log("Still more to spawn!");
            StartCoroutine(SpawnCycle());
        }
        else
        {
            spawnCycleOn = false;
        }
    }

    [Server]
    public virtual void CmdSpawnObject()
    {
        Debug.Log("Base Spawn...");
        GameObject newSpawn = Instantiate(spawnedObject, mySpawnPoint.transform.position, Quaternion.identity);
        NetworkServer.Spawn(newSpawn);
        mySpawns.Add(newSpawn);
    }


    public void checkMySpawns()
    {
        checkingSpawns = true;
        for (int count = 0; count < mySpawns.Count; count++)
        {
            if (mySpawns[count] == null)
            {
                mySpawns.Remove(mySpawns[count]);
            }
        }
        checkingSpawns = false;
    }
}
