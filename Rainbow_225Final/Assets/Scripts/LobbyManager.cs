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
    public GameObject selectionMenu;
    public GameObject hostMenu;
    public GameObject joinMenu;
    public GameObject serverMenu;

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

    #region Entry Assets
    public TMP_InputField roomNameField;
    public TMP_Dropdown gamemodeDrop;
    public TMP_Dropdown teamtypeDrop;
    public TMP_InputField maxPlayerField;
    public TMP_InputField timeLimitField;

    public TMP_InputField ipField;
    #endregion

    int currentMenu = 0;
    public RoomInfo myRoomInfo;
    

    public string roomName = "Default";
    public int maxPlayers = 4;
    public int timeLimit = 300;
    public int gamemode;
    public int teamtype;
    public string map = "InGame";

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

        string name = RainbowNetwork.instance.myUsername;
        if (name != null)
        {
            roomNameField.text = name + "'s Room";
        }
        else
        {
            roomNameField.text = roomName;
        }
        
        maxPlayerField.text = maxPlayers.ToString();
        timeLimitField.text = timeLimit.ToString();

        foreach (Gamemode gm in RainbowNetwork.instance.gamemodeList)
        {
            gamemodeDrop.options.Add(new TMP_Dropdown.OptionData() { text = gm.name, image = gm.icon});
        }

        ipField.text = RainbowNetwork.instance.networkAddress;

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
        /*if (roomSelected)
        {
            enterButton.interactable = true;
            roomSelectedName.text = roomSelected.slotName.text.ToString();
            roomSelectedGamemode.text = roomSelected.gamemodeName;
        }
        else
        {
            enterButton.interactable = false;
            roomSelectedName.text = "Select a room to enter.";
            roomSelectedGamemode.text = "";
        } */  
    }

    #region Button Clicks
    public void OnHostClick()
    {
        hostMenu.SetActive(true);
        selectionMenu.SetActive(false);
        currentMenu = 1;
    }

    public void OnJoinLocalClick()
    {
        joinMenu.SetActive(true);
        selectionMenu.SetActive(false);
        currentMenu = 2;
    }

    public void OnJoinServerClick()
    {
        serverMenu.SetActive(true);
        selectionMenu.SetActive(false);
        currentMenu = 3;
    }

    public void OnGoButtonClick()
    {
        if (currentMenu == 0)   // MAIN SELECTION
        {

        }
        else if (currentMenu == 1)  // CREATE ROOM
        {
            HostRoom();
        }
        else if (currentMenu == 2)  // JOIN ROOM via IPV4
        {
            JoinRoom(ipField.text);
        }
        else if (currentMenu == 3)  // JOIN ROOM via SERVER
        {
            JoinServer();
        }
    }

    public void OnBackButtonClick()
    {
        if (currentMenu == 1)   // MAIN SELECTION
        {
            SceneManager.LoadScene("Title");
        }
        else if (currentMenu == 1)  // CREATE ROOM
        {
            joinMenu.SetActive(false);
            selectionMenu.SetActive(true);
        }
        else if (currentMenu == 2)  // JOIN ROOM via IPV4
        {
            joinMenu.SetActive(false);
            selectionMenu.SetActive(true);
        }
        else if (currentMenu == 3)  // JOIN ROOM via SERVER
        {
            serverMenu.SetActive(false);
            selectionMenu.SetActive(true);
        }
        currentMenu = 0;
    }

    // Create Room Menu
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

    #endregion

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



   

    public void HostRoom()
    {
        if (RainbowNetwork.instance != null)
        {
            roomName = roomNameField.text;
            gamemode = gamemodeDrop.value;
            Debug.Log("[LOBBY] Creating room " + roomName.ToString() + ". Gamemode: " + gamemode);
            RainbowNetwork.instance.StartHost();
            //PhotonManager.instance.CreateRoom(roomName, maxPlayers, timeLimit, gamemode, map);
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: Network not found");
        }

    }

    public void JoinRoom(string ip)
    {
        if (RainbowNetwork.instance != null)
        {
            RainbowNetwork.instance.networkAddress = ip;
            RainbowNetwork.instance.StartClient();
            //PhotonNetwork.JoinRoom(roomSelected.slotName.text.ToString());
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: Network not found");
        }
    }

    public void JoinServer()
    {
        Debug.LogError("Not yet implemented.");
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

    public void startGame(string map) // start game
    {
        PhotonManager.instance.StartGame(map);
    }
}
