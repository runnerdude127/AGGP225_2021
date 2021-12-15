using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject timer;

    [SerializeField]
    TMP_Text timeText;
    public static GameManager instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {

    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Title");
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
        //ChatroomManager.instance.consoleMessage("LeaveGameMessage");
    }

    public GameObject getSpawn()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
