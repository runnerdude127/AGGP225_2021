using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    #region Main Menu UI
    public Button playButton;
    public Button exitButton;
    public TMP_Text log;
    public TMP_InputField usernameField;
    public CharacterSelector selector;

    CharClass playerClass;
    #endregion

    public static MainMenuUI instance { get; private set; } // SINGLETON INSTANCE

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
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if (usernameField)
        {
            usernameField.text = RainbowNetwork.instance.myUsername;
        }
    }

    #region UI Updates
    private void Update()
    {
        if (usernameField.text != "")
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    public void UpdateLog(string _log)
    {
        if (_log == null)
        {
            return;
        }
        else
        {
            log.text = _log;
        }
    }
    #endregion

    #region Button Clicks
    public void OnPlayClick()
    {        
        if (RainbowNetwork.instance != null)
        {
            if (!string.IsNullOrEmpty(usernameField.text))
            {
                RainbowNetwork.instance.myUsername = usernameField.text;
                setClass();
                SceneManager.LoadScene("MirrorLobby");
            }
            else
            {
                UpdateLog("You must have a username!");
            }
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: Network not found");
        }
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void setClass()
    {
        if (selector)
        {
            RainbowNetwork.instance.classID = selector.GetClass();
        }
    }
    #endregion
}
