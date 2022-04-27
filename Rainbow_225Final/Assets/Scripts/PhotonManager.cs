using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// current game version
    /// </summary>
    string gameVersion = "indev 1.0";

    public string myUsername = "Default";
    //public CharacterClass myClass;
    
    public bool canConnect;

    public List<Gamemode> gamemodeList;
    public List<CharClass> classList;
    public int classID = 0;
    public List<Weapon> weaponList;
    public int weaponID = 0;
    public List<Bullet> bulletList;

    //string gameplayLevel = "InGame";
    public int timer;

    List<RoomInfo> roomsAware;

    public const string GAMEMODE = "GM";
    public const string TIMELIMIT = "TL";
    public const string MAP = "MP";

    public static PhotonManager instance { get; private set; }
    void Awake()
    {       
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        roomsAware = new List<RoomInfo>();
    }

    void Start()
    {
        MainMenuUI.instance.UpdateLog("Connecting...");
        Connect();
    } 

    /// <summary>
    /// connects user to master server
    /// </summary>
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }    
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected! [PhotonManager][OnConnectedToMaster]");
        MainMenuUI.instance.UpdateLog("Connected to Master");
        canConnect = true;
        //CreateRoom();
    }

    #region Photon Callbacks
    public void CreateRoom(string roomName, int maxPlayers, int timeLim, int gamemode, string map)
    {
        Debug.Log("[PHOTONMAN] Got request for room " + roomName.ToString() + ". Gamemode: " + gamemode);
        string[] LobbyOptions = new string[2];
        LobbyOptions[0] = GAMEMODE;
        LobbyOptions[1] = MAP;

        RoomOptions roomCreated = new RoomOptions();
        roomCreated.MaxPlayers = (byte)maxPlayers;

        roomCreated.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomCreated.CustomRoomProperties.Add(GAMEMODE, gamemode);
        roomCreated.CustomRoomProperties.Add(MAP, map);
        roomCreated.CustomRoomProperties.Add(TIMELIMIT, timeLim);

        roomCreated.CustomRoomPropertiesForLobby = LobbyOptions;

        Debug.Log("Creating room... [PhotonManager][CreateRoom]");
        MainMenuUI.instance.UpdateLog("Creating room...");
        PhotonNetwork.CreateRoom(roomName, roomCreated, null);
    }
    public void JoinRandomRoom()
    {
        Debug.Log("Searching for a room... [PhotonManager][JoinRandomRoom]");
        MainMenuUI.instance.UpdateLog("Searching for a room...");
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
        Debug.Log("Leaving... [PhotonManager][LeaveRoom]");
        MainMenuUI.instance.UpdateLog("Leaving...");
        SceneManager.LoadScene("Title");
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame(string map)
    {
        Debug.Log("Starting the Game... [PhotonManager][StartGame]");
        MainMenuUI.instance.UpdateLog("Starting...");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(map);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room '" + PhotonNetwork.CurrentRoom.Name + "' created. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Room '" + PhotonNetwork.CurrentRoom.Name + "' created.");

        StartGame((string)PhotonNetwork.CurrentRoom.CustomProperties[MAP]);

        //LobbyManager.instance.startGame((string)PhotonNetwork.CurrentRoom.CustomProperties[MAP]);
        //LobbyManager.instance.openMenu.SetActive(true);
        //LobbyManager.instance.creationMenu.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomData in roomList)
        {
            if (roomData.IsVisible && roomData.IsOpen && !roomData.RemovedFromList)
            {
                if (roomsAware.Contains(roomData))
                {
                    int slotnum = roomsAware.IndexOf(roomData);
                    roomsAware.Remove(roomData);
                    roomsAware.Insert(slotnum, roomData);
                }
                else
                {
                    roomsAware.Insert(roomsAware.Count, roomData);
                } 
            }
            else
            {
                if (roomsAware.Contains(roomData))
                {
                    roomsAware.Remove(roomData);
                }
            }
        }

        Debug.Log("Refreshing...");
        foreach (Transform child in LobbyManager.instance.roomlist)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Rooms: " + roomsAware.Count);
        if (roomsAware.Count > 0)
        {
            foreach (RoomInfo roomData in roomsAware)
            {
                Debug.Log("Room data for " + roomData.Name + "... Gamemode: " + (int)roomData.CustomProperties[GAMEMODE]);
                LobbyManager.instance.insertRoom(roomData, (int)roomData.CustomProperties[GAMEMODE]);
            }
        } 
    }

    public override void OnJoinedRoom()
    {        
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LoadLevel(gameplayLevel);
        }
        LobbyManager.instance.insertPlayer();
        Debug.Log("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'.");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room. [PhotonManager][OnLeftRoom]");
        MainMenuUI.instance.UpdateLog("Left the room.");
    }
    #endregion

    #region Photon Failures
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected! Reason: " + cause + " [PhotonManager][OnDisconnected]");
        MainMenuUI.instance.UpdateLog("Disconnected! Reason: " + cause);
        //SceneManager.LoadScene("Title");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed. Reason: " + message + " [PhotonManager][OnCreatedFailed]");
        MainMenuUI.instance.UpdateLog("Room creation failed. Reason: " + message);
        //JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Connection failed. Reason: " + message + " [PhotonManager][OnJoinRandomFailed]");
        MainMenuUI.instance.UpdateLog("Connection failed. Reason: " + message);
        //Debug.Log("Creating a failsafe room... [PhotonManager][OnCreatedRoom]");
        //CreateRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Connection failed. Reason: " + message + " [PhotonManager][OnJoinRoomFailed]");
        //MainMenuUI.instance.UpdateLog("Connection failed. Reason: " + message);
        //Debug.Log("Creating a failsafe room... [PhotonManager][OnCreatedRoom]");
        //CreateRoom();
    }
    #endregion

    [PunRPC]
    void UsernameRPC(string _username, string _chat)
    {
        //Username.instance.usernameText.text = _username + ": " + _chat;
    }

    public int getPlayerClass()
    {
        return classID;
    }

    public Weapon GetWeapon(int ID)
    {
        if (ID != -1)
        {
            return weaponList[ID];
        }
        else
        {
            return weaponList[0]; // failsafe
        }
    }

    public Bullet GetBullet(int ID)
    {
        return bulletList[ID];
    }
}
