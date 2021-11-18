using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviour
{
    public GameObject playerSlotPrefab;
    public RectTransform playerlist;
    public GameObject roomSlotPrefab;
    public RectTransform roomlist;
    public TMP_InputField roomNameField;
    public TMP_Dropdown gamemodeDrop;
    public TMP_InputField maxPlayerField;
    public TMP_InputField timeLimitField;

    public GameObject openMenu;
    public GameObject creationMenu;
    public GameObject listMenu;

    public string roomName = "MyCoolRoom";
    public int gamemode;
    public int maxPlayers = 4;
    public int timeLimit = 300;

    public static LobbyManager instance { get; private set; } // SINGLETON INSTANCE

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

    public void Start()
    {
        roomNameField.text = roomName;
        maxPlayerField.text = maxPlayers.ToString();
        timeLimitField.text = timeLimit.ToString();

        if (MainMenuUI.instance.titleButtonClicked == true)
        {
            listMenu.SetActive(true);
            creationMenu.SetActive(false);
            openMenu.SetActive(false);
        }
        else
        {
            listMenu.SetActive(false);
            creationMenu.SetActive(true);
            openMenu.SetActive(false);
        }
    }

    public void Update()
    {
        roomName = roomNameField.text;
        gamemode = gamemodeDrop.value;
    }

    public void insertPlayer()
    {
        if (playerSlotPrefab)
        {
            PhotonNetwork.Instantiate(playerSlotPrefab.name, playerlist.transform.position, Quaternion.identity);
            Debug.Log("Player Slot inserted");
        }
        else
        {
            Debug.Log("playerSlotPrefab not set. [insertPlayer][Start]");
        }
    }

    public void insertRoom(Room roomInfo, int gamemode)
    {
        if (roomSlotPrefab)
        {
            GameObject newSlot = PhotonNetwork.Instantiate(roomSlotPrefab.name, roomlist.transform.position, Quaternion.identity);
            RoomInfoSlot slotInfo = newSlot.GetComponent<RoomInfoSlot>();
            newSlot.transform.SetParent(roomlist);
            slotInfo.infoSet(roomInfo.Name, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            slotInfo.roomExtras(gamemode, roomInfo.PlayerCount, roomInfo.MaxPlayers);
            Debug.Log("Room Slot inserted");
        }
        else
        {
            Debug.Log("roomSlotPrefab not set. [insertRoom][Start]");
        }
    }

    public void valChangeManual()
    {
        int newPlayers = int.Parse(maxPlayerField.text);
        int newTime = int.Parse(timeLimitField.text);

        if (newPlayers < 2)
        {
            newPlayers = 2;
        }
        else if (newPlayers > 20)
        {
            newPlayers = 20;
        }

        if (newTime < 10)
        {
            newTime = 10;
        }
        else if (newTime > 9999)
        {
            newTime = 9999;
        }

        maxPlayers = newPlayers;
        timeLimit = newTime;

        maxPlayerField.text = maxPlayers.ToString();
        timeLimitField.text = timeLimit.ToString();
    }

    public void valChangeMPButton(int op)
    {
        int newPlayers = maxPlayers + op;

        if (newPlayers < 2)
        {
            maxPlayers = 2;
        }
        else if (newPlayers > 20)
        {
            maxPlayers = 20;
        }
        else
        {
            maxPlayers += op;
        }
        maxPlayerField.text = maxPlayers.ToString();
    }

    public void valChangeTLButton(int op)
    {
        int newTime = timeLimit + op;

        if (newTime < 0)
        {
            timeLimit = 10;
        }
        else if (newTime > 9999)
        {
            timeLimit = 9999;
        }
        else
        {
            timeLimit += op;
        }
        timeLimitField.text = timeLimit.ToString();
    }

    public void createRoom()
    {
        if (PhotonManager.instance != null)
        {
            PhotonManager.instance.CreateRoom(roomName, maxPlayers, timeLimit, gamemode);
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: PhotonManager not found");
        }
    }

    public void joinRoom()
    {
        if (PhotonManager.instance != null)
        {
            // join a specific room
        }
        else
        {
            Debug.LogError("Unable to join room. Reason: PhotonManager not found");
        }
    }

    public void randomRoom()
    {
        if (PhotonManager.instance != null)
        {
            PhotonManager.instance.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("Unable to join room. Reason: PhotonManager not found");
        }
    }

    public void returnToMenu() // leave creation
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void leaveRoom() // leave room
    {
        PhotonManager.instance.LeaveRoom();
    }

    public void startGame() // start game
    {
        PhotonManager.instance.StartGame();
    }
}
