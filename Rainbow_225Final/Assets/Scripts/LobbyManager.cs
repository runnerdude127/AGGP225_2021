using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviour
{
    public GameObject mainLobby;
    public GameObject creationMenu;

    #region lobby assets
    public GameObject playerSlotPrefab;
    public RectTransform playerlist;

    public GameObject roomSlotPrefab;
    public RectTransform roomlist;

    public TMP_Text roomSelectedName;
    public TMP_Text roomSelectedGamemode;
    public RoomInfoSlot roomSelected;
    public Button enterButton;
    #endregion

    #region create assets
    public TMP_InputField roomNameField;
    public TMP_Dropdown gamemodeDrop;
    public TMP_InputField maxPlayerField;
    public TMP_InputField timeLimitField;
    #endregion


    public RoomInfo myRoomInfo;
    public int myGamemode;

    public string roomName = "Default";
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
        PhotonNetwork.JoinLobby();

        roomNameField.text = roomName;
        maxPlayerField.text = maxPlayers.ToString();
        timeLimitField.text = timeLimit.ToString();
        
        /*if (MainMenuUI.instance.titleButtonClicked == true)
        {
            PhotonNetwork.JoinLobby();
            listMenu.SetActive(true);
            creationMenu.SetActive(false);
            openMenu.SetActive(false);
        }
        else
        {
            listMenu.SetActive(false);
            creationMenu.SetActive(true);
            openMenu.SetActive(false);
        }*/
    }

    public void Update()
    {
        roomName = roomNameField.text;
        myGamemode = gamemodeDrop.value;

        if (roomSelected)
        {
            enterButton.interactable = true;
            roomSelectedName.text = roomSelected.slotName.text.ToString();
            roomSelectedGamemode.text = "temp";
        }
        else
        {
            enterButton.interactable = false;
            roomSelectedName.text = "Select a room to enter.";
            roomSelectedGamemode.text = "";
        }   
    }

    public void insertPlayer()
    {
        if (playerSlotPrefab)
        {
            //PhotonNetwork.Instantiate(playerSlotPrefab.name, playerlist.transform.position, Quaternion.identity);
            //Debug.Log("Player Slot inserted");
        }
        else
        {
            Debug.Log("playerSlotPrefab not set. [insertPlayer][Start]");
        }
    }

    public void insertRoom(RoomInfo roomData, int gamemode)
    {
        if (roomSlotPrefab)
        {
            GameObject newSlot = Instantiate(roomSlotPrefab, roomlist.transform.position, Quaternion.identity);
            RoomInfoSlot thisSlotData = newSlot.GetComponent<RoomInfoSlot>();
            thisSlotData.infoSet(roomData.Name);
            thisSlotData.roomExtras(gamemode, roomData.PlayerCount, roomData.MaxPlayers);
            //PhotonNetwork.Instantiate(roomSlotPrefab.name, roomlist.transform.position, Quaternion.identity);
            //Debug.Log("Room Slot inserted");
        }
        else
        {
            Debug.Log("roomSlotPrefab not set. [insertRoom][Start]");
        }
    }



    public void valChangeManual()
    {
        int newPlayers;
        int newTime;
        
        if (timeLimitField.text == "")
        {
            timeLimitField.text = timeLimit.ToString();
        }

        int.TryParse(maxPlayerField.text, out newPlayers);
        int.TryParse(timeLimitField.text, out newTime);
        

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
            PhotonManager.instance.CreateRoom(roomName, maxPlayers, timeLimit, myGamemode);
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
            PhotonNetwork.JoinRoom(roomSelected.slotName.text.ToString());
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

    public void returnToMenu() // leave lobby
    {
        SceneManager.LoadScene("Title");
    }

    public void beginCreation()
    {
        creationMenu.SetActive(true);
        mainLobby.SetActive(false);
    }

    public void leaveCreation()
    {
        creationMenu.SetActive(false);
        mainLobby.SetActive(true);
    }

    public void startGame() // start game
    {
        PhotonManager.instance.StartGame();
    }
}
