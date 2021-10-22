using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    #region InRoom UI
    public bool paused = false;
    public bool chatting = false;
    public GameObject pauseMenu;
    public TMP_InputField chatBox;

    public Image colorUI;
    public Meter thisPlayerHealth;
    public Image healthColor;
    public Image infectColor;
    public Image chargeColor;

    public Meter thisPlayerCharge;

    public Animator colorBlockAnim;
    Animation colorchange;
    #endregion

    public bool playerMouseInput = false;
    public bool playerKeyInput = false;

    public static PlayerGUI instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        chatBox.interactable = false;
    }

    private void Start()
    {
        colorBlockAnim = colorUI.GetComponent<Animator>();
    }

    #region UI Updates
    private void Update()
    {
        inputStateUpdate();
        inputProcessing();
        inputState();
        pauseMenu.SetActive(paused);    
    }
    #endregion

    void inputProcessing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.T) && chatting == false)
        {
            chatBox.interactable = true;
            chatBox.ActivateInputField();
            ChatboxSelect();
        }
        

        if (Input.GetKeyDown(KeyCode.Return) && chatting == false)
        {
            ChatroomManager.instance.submitMessage();
            chatBox.interactable = false;
        }
    }

    void inputStateUpdate()
    {
        if (paused == true)
        {
            playerMouseInput = false;
            playerKeyInput = false;
        }
        else if (chatting == true)
        {
            playerKeyInput = false; 
        }
        else
        {
            playerMouseInput = true;
            playerKeyInput = true;
        }
    }

    void inputState()
    {
        if (playerMouseInput == true)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }

    public void PauseMenu()
    {
        if (paused == false)
        {
            paused = true;
        }
        else
        {
            paused = false;
        }
    }

    public void ChatboxSelect()
    {
        chatting = true;
    }

    public void ChatboxDeselect()
    {
        chatting = false;
    }

    public void leaveGame()
    {
        PhotonManager.instance.LeaveRoom();
    }
}
