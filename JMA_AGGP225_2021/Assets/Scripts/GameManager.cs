using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    //public Camera playerCamera;
    public GameObject playerPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject timer;

    private void Awake()
    {

    }

    private void Update()
    {
        
    }

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
            Debug.Log("instantiate has been called from " + PhotonManager.instance.myUsername);
        }
        else
        {
            Debug.Log("playerPrefab not set. [GameManager][Start]");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ChatroomManager.instance.consoleMessage("LeaveGameMessage");
    }
}