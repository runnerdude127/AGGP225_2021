using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MainMenuUI : MonoBehaviour
{
    #region Main Menu UI
    [SerializeField]
    Button playButton;

    [SerializeField]
    Button exitButton;

    [SerializeField]
    TMP_Text log;

    [SerializeField]
    TMP_InputField usernameField;

    [SerializeField]
    CharacterSelector selector;
    CharacterClass playerClass;

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
            usernameField.text = PhotonManager.instance.myUsername;
        }
    }

    #region UI Updates
    private void Update()
    {
        if (PhotonNetwork.IsConnected && usernameField.text != "")
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
        if (PhotonManager.instance != null)
        {
            if (!string.IsNullOrEmpty(usernameField.text))
            {
                PhotonManager.instance.myUsername = usernameField.text;
                setClass();
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                UpdateLog("You must have a username!");
            }
        }
        else
        {
            Debug.LogError("Unable to create room. Reason: PhotonManager not found");
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
            PhotonManager.instance.classID = selector.GetClass();
        }
    }
    #endregion
}
