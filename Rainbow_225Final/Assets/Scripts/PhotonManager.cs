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
    public int classID;
    public bool canConnect;

    RoomOptions myRoom;
    public List<CharacterClass> classList;
    string gameplayLevel = "InGame";
    public int timer;
    int gm;

    public const string GAMEMODE = "GM";
    //public const string TIMELIMIT = "TL";

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
    public void CreateRoom(string roomName, int maxPlayers, int timeLim, int gamemode)
    {
        RoomOptions roomCreated = new RoomOptions();
        roomCreated.MaxPlayers = (byte)maxPlayers;

        roomCreated.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomCreated.CustomRoomProperties.Add(GAMEMODE, gamemode);
        //roomCreated.CustomRoomProperties.Add(TIMELIMIT, timeLim);
        myRoom = roomCreated;

        Debug.Log("Creating room... [PhotonManager][CreateRoom]");
        MainMenuUI.instance.UpdateLog("Creating room...");
        PhotonNetwork.CreateRoom(roomName, myRoom);
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

        /*if (CameraManagaer.instance)
        {
            CameraManagaer.instance.isFollowing = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }*/
        
        PhotonNetwork.LeaveRoom();
    }

    /*public void GetRoomSize(string name)
    {
        PhotonNetwork.
    }*/

    public void StartGame()
    {
        Debug.Log("Starting the Game... [PhotonManager][StartGame]");
        MainMenuUI.instance.UpdateLog("Starting...");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameplayLevel);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room '" + PhotonNetwork.CurrentRoom.Name + "' created. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Room '" + PhotonNetwork.CurrentRoom.Name + "' created.");

        LobbyManager.instance.startGame();
        //LobbyManager.instance.openMenu.SetActive(true);
        //LobbyManager.instance.creationMenu.SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform child in LobbyManager.instance.roomlist)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo roomData in roomList)
        {
            LobbyManager.instance.insertRoom(roomData, 1/*(int)roomData.CustomProperties[GAMEMODE]*/);
        }
    }

    public override void OnJoinedRoom()
    {        
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LoadLevel(gameplayLevel);
        }
        //LobbyManager.instance.openMenu.SetActive(true);
        //LobbyManager.instance.listMenu.SetActive(false);
        LobbyManager.instance.insertPlayer();
        Debug.Log("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'.");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room. [PhotonManager][OnLeftRoom]");
        MainMenuUI.instance.UpdateLog("Left the room.");
        //SceneManager.LoadScene("Title");
    }
    #endregion

    #region Photon Failures
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected! Reason: " + cause + " [PhotonManager][OnDisconnected]");
        MainMenuUI.instance.UpdateLog("Disconnected! Reason: " + cause);
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
}
