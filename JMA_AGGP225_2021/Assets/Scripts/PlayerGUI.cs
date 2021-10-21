using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    #region InRoom UI
    bool paused = false;
    bool chatting = false;
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

        if (Input.GetKeyDown(KeyCode.Return))
        {      
            if (chatting == true)
            {
                ChatroomManager.instance.submitMessage();
                Debug.Log("SWOINK");
                ChatboxSelect();
            }
            else
            {
                chatBox.ActivateInputField();
                ChatboxSelect();
            } 
        }
    }

    void inputStateUpdate()
    {
        if (paused == true)
        {
            playerMouseInput = false;
            playerKeyInput = false;
        }
        else
        {
            playerMouseInput = true;
            playerKeyInput = true;
        }

        if (chatting == true)
        {
            playerKeyInput = false;         
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
        if (chatting == false)
        {
            chatting = true;
        }
        else
        {
            chatting = false;
        }
    }

    public void leaveGame()
    {
        PhotonManager.instance.LeaveRoom();
    }
}
