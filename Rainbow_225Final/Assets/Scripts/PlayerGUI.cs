using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    public bool paused = false;
    public bool playerInput = false;
    public bool isDead = false;

    public GameObject pauseMenu;
    public ChatboxUI chatBox;
    public TextMeshProUGUI timerText;

    public Meter thisPlayerHealth;
    public Meter thisPlayerCharge;
    public Image portraitIcon;

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

    private void Update()
    {
        inputStateUpdate();
        pauseMenu.SetActive(paused);
    }

    void inputStateUpdate()
    {
        if (paused == true)
        {
            playerInput = false;
        }
        else if (chatBox && chatBox.chatting == true)
        {
            playerInput = false;
        }
        else
        {
            if (isDead)
            {
                playerInput = false;
            }
            else
            {
                playerInput = true;
            }
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

    public void playerDeath(bool deadstate)
    {
        isDead = deadstate;
    }

    public void leaveGame()
    {
        //ChatroomManager.instance.consoleMessage("LeaveGameMessage");
        PhotonManager.instance.LeaveRoom();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
