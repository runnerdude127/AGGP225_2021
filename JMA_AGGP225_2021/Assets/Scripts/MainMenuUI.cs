using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Main Menu UI
    [SerializeField]
    Button createButton;

    [SerializeField]
    Button randomButton;

    [SerializeField]
    TMP_Text log;

    [SerializeField]
    TMP_InputField usernameField;
    #endregion

    #region Game UI

    [SerializeField]
    TMP_Text timeText;

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
        //colorUIRenderer = colorUI.GetComponent<Renderer>();
    }

    #region UI Updates
    private void Update()
    {
        if (createButton && randomButton)
        {
            if (PhotonManager.instance.canConnect)
            {
                createButton.interactable = true;
                randomButton.interactable = true;
            }
            else
            {
                createButton.interactable = false;
                randomButton.interactable = false;
            }
        }

        if (timeText)
        {
            timeText.text = PhotonManager.instance.timer.ToString();
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
    public void OnCreateRoomClick()
    {        
        if (PhotonManager.instance != null)
        {
            if (!string.IsNullOrEmpty(usernameField.text))
            {
                PhotonManager.instance.myUsername = usernameField.text;
                PhotonManager.instance.CreateRoom();
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

    public void OnJoinRoomClick()
    {     
        if (PhotonManager.instance != null)
        {
            if (!string.IsNullOrEmpty(usernameField.text))
            {
                PhotonManager.instance.myUsername = usernameField.text;
                PhotonManager.instance.JoinRandomRoom();
            }
            else
            {
                UpdateLog("You must have a username!");
            }           
        }
        else
        {
            Debug.LogError("Unable to join room. Reason: PhotonManager not found");
        }
    }

    public void OnLeaveRoomClick()
    {
        Debug.Log("i love clicking on stupid buttons");
        PhotonManager.instance.LeaveRoom();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
    #endregion
}
