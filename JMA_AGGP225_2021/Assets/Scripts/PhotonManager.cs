using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// current game version
    /// </summary>
    string gameVersion = "really really early";

    public string myUsername;
    public bool canConnect;

    RoomOptions roomOptions = new RoomOptions();
    string gameplayLevel = "InRoom";

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
        roomOptions.MaxPlayers = 4;
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
    public void CreateRoom()
    {
        Debug.Log("Creating room... [PhotonManager][CreateRoom]");
        MainMenuUI.instance.UpdateLog("Creating room...");
        PhotonNetwork.CreateRoom("MyCoolRoom", roomOptions);
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
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room '" + PhotonNetwork.CurrentRoom.Name + "' created. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Room '" + PhotonNetwork.CurrentRoom.Name + "' created.");
    }
    public override void OnJoinedRoom()
    {        
        PhotonNetwork.LoadLevel(gameplayLevel);
        Debug.Log("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'. [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'.");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room. [PhotonManager][OnLeftRoom]");
        MainMenuUI.instance.UpdateLog("Left the room.");
        PhotonNetwork.LoadLevel("SampleScene");
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
        Debug.Log("Room creation failed. Reason: " + message + " [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Room creation failed. Reason: " + message);
        JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Connection failed. Reason: " + message + " [PhotonManager][OnCreatedRoom]");
        MainMenuUI.instance.UpdateLog("Connection failed. Reason: " + message);
        //Debug.Log("Creating a failsafe room... [PhotonManager][OnCreatedRoom]");
        //CreateRoom();
    }
    #endregion

    [PunRPC]
    void UsernameRPC(string _username, string _chat)
    {
        Username.instance.usernameText.text = _username + ": " + _chat;
    }
}