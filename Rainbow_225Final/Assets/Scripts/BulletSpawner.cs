using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletSpawner : MonoBehaviour
{
    public Player parent;
    void Start()
    {
        
        Destroy(gameObject);
    }
}
