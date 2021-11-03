using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField roomNameField;
    public TMP_Dropdown gamemodeDrop;
    public TMP_InputField maxPlayerField;
    public TMP_InputField timeLimitField;
 
    public string roomName = "MyCoolRoom";
    public int gamemode;
    public int maxPlayers = 4;
    public int timeLimit = 300;

    public void Start()
    {
        roomNameField.text = roomName;
        maxPlayerField.text = maxPlayers.ToString();
        timeLimitField.text = timeLimit.ToString();
    }

    public void Update()
    {
        roomName = roomNameField.text;
        gamemode = gamemodeDrop.value;
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
            RoomOptions roomCreated = new RoomOptions();
            roomCreated.MaxPlayers = (byte)maxPlayers;

            PhotonManager.instance.CreateRoom(roomName, roomCreated, timeLimit, gamemode);
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: PhotonManager not found");
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
}
