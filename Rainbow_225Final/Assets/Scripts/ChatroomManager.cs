using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class ChatroomManager : MonoBehaviour
{
    public TMP_InputField messageInput;
    public TMP_Text field;

    public static ChatroomManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void submitMessage()
    {
        if(!string.IsNullOrEmpty(messageInput.text))
        {
            gameObject.GetPhotonView().RPC("UpdateChatroom", RpcTarget.AllBuffered, PhotonManager.instance.myUsername, messageInput.text);
            messageInput.text = "";
        }
    }

    public void consoleMessage(string status)
    {
        gameObject.GetPhotonView().RPC(status, RpcTarget.AllBuffered, PhotonManager.instance.myUsername);
    }


    [PunRPC]
    void UpdateChatroom(string _username, string _chat)
    {
        field.text += _username + ": " + _chat + "\n";
    }

    [PunRPC]
    void JoinGameMessage(string _username)
    {
        field.text += _username + " joined the game." + "\n";
    }

    [PunRPC]
    void LeaveGameMessage(string _username)
    {
        field.text += _username + " left the game." + "\n";
    }
}
