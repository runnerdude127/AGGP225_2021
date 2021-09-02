using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnCreateRoomClick()
    {
        PhotonManager.instance.CreateRoom();
    }

    public void OnJoinRoomClick()
    {
        PhotonManager.instance.JoinRandomRoom();
    }

    public void OnLeaveRoomClick()
    {
        PhotonManager.instance.LeaveRoom();
    }
}
