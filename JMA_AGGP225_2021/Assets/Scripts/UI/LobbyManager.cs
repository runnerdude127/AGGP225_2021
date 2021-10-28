using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public void returnToMenu() // leave creation
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void leaveRoom() // leave room
    {
        PhotonManager.instance.LeaveRoom();
    }
}
