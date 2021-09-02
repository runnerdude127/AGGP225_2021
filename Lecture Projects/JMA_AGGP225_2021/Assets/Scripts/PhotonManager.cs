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
        //CreateRoom();
    }

    #region Photon Callbacks
    public void CreateRoom()
    {
        Debug.Log("Creating room... [PhotonManager][CreateRoom]");
        PhotonNetwork.CreateRoom("MyCoolRoom", roomOptions);
    }

    public void JoinRandomRoom()
    {
        Debug.Log("Searching for a room... [PhotonManager][JoinRandomRoom]");
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        Debug.Log("Leaving... [PhotonManager][LeaveRoom]");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room '" + PhotonNetwork.CurrentRoom.Name + "' created. [PhotonManager][OnCreatedRoom]");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to Room '" + PhotonNetwork.CurrentRoom.Name + "'. [PhotonManager][OnCreatedRoom]");
        PhotonNetwork.LoadLevel(gameplayLevel);       
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room. [PhotonManager][OnLeftRoom]");
        PhotonNetwork.LoadLevel("SampleScene");
    }
    #endregion

    #region Photon Failures

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected! Reason: " + cause + " [PhotonManager][OnDisconnected]");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed. Reason: " + message + " [PhotonManager][OnCreatedRoom]");
        JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Connection failed. Reason: " + message + " [PhotonManager][OnCreatedRoom]");
        Debug.Log("Creating a failsafe room... [PhotonManager][OnCreatedRoom]");
        CreateRoom();
    }

    #endregion
}
