using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    #region Main Menu UI
    [SerializeField]
    Button createButton;

    [SerializeField]
    Button joinButton;

    [SerializeField]
    Button randomButton;

    [SerializeField]
    public bool titleButtonClicked;

    [SerializeField]
    TMP_Text log;

    [SerializeField]
    TMP_InputField usernameField;

    [SerializeField]
    Image colorRef;
    Color playerStartColor;

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
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if (usernameField)
        {
            usernameField.text = PhotonManager.instance.myUsername;
        }
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
                joinButton.interactable = true;
                randomButton.interactable = true;
            }
            else
            {
                createButton.interactable = false;
                joinButton.interactable = false;
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
                playerCreate();
                //PhotonManager.instance.CreateRoom();
                titleButtonClicked = false;
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

    public void OnJoinRoomClick()
    {     
        if (PhotonManager.instance != null)
        {
            if (!string.IsNullOrEmpty(usernameField.text))
            {
                PhotonManager.instance.myUsername = usernameField.text;
                playerCreate();
                titleButtonClicked = true;
                SceneManager.LoadScene("Lobby");                
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

    public void playerCreate()
    {
        if (colorRef)
        {
            playerStartColor = colorRef.color;
            PhotonManager.instance.myColor = playerStartColor;
        }
        else
        {
            PhotonManager.instance.myColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
    #endregion
}
