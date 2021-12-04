using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatboxUI : MonoBehaviour
{
    public bool chatting;
    public TMP_InputField chatInput;

    private void Awake()
    {
        chatInput.interactable = false;
        chatting = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && chatting == false)
        {
            chatInput.interactable = true;
            chatInput.ActivateInputField();
            chatting = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) && chatting == true)
        {
            ChatroomManager.instance.submitMessage();
            chatInput.interactable = false;
            chatting = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) && chatting == false)
        {
            //ChatroomManager.instance.submitMessage();
            //chatInput.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && chatting == true)
        {
            chatInput.interactable = false;
            chatting = false;
        }
    }
}
