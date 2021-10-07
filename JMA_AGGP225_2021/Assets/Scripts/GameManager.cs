using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    //public Camera playerCamera;
    public GameObject playerPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("SampleScene");
            return;
        }

        if (playerPrefab)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else
        {
            Debug.Log("playerPrefab not set. [GameManager][Start]");
        }
    }
}