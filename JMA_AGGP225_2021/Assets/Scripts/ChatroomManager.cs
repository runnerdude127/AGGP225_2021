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
            Debug.Log("1: " + gameObject.name);
            Debug.Log("3: " + PhotonManager.instance.myUsername);
            Debug.Log("4: " + messageInput.text);
            gameObject.GetPhotonView().RPC("UpdateChatroom", RpcTarget.AllBuffered, PhotonManager.instance.myUsername, messageInput.text);
            messageInput.text = "";
        }
    }


    [PunRPC]
    void UpdateChatroom(string _username, string _chat)
    {
        Debug.Log(_username + ": " + _chat + "\n");
        field.text += _username + ": " + _chat + "\n";
    }
}
