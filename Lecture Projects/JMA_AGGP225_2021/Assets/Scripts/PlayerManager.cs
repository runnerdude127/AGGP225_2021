using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public Color color;

    public float speed = 5f;
    public CharacterController characterController;
    public Material playerColor;
    float horizontal;
    float vertical;
    float velocity;
    CameraManagaer cam;
    Vector3 camDir;

    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        playerColor = gameObject.GetComponent<MeshRenderer>().material;
        cam = gameObject.GetComponent<CameraManagaer>();



        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void Update()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            camDir = cam.GetCameraDirection();
            if (cam != null)
            {
                cam.OnStartFollowing();
            }

            if (characterController.isGrounded)
            {
                horizontal = Input.GetAxis("Horizontal") * speed;
                vertical = Input.GetAxis("Vertical") * speed;
            }

            if (characterController.isGrounded)
            {
                velocity = 0;
            }
            else
            {
                velocity -= 9.81f * Time.deltaTime;
            }

            gameObject.transform.eulerAngles = camDir;
            characterController.Move((gameObject.transform.right * horizontal + gameObject.transform.forward * vertical) * Time.deltaTime);
            characterController.Move(new Vector3(0, velocity, 0));

            // changes local user's color to apply
            if (Input.GetKeyDown(KeyCode.Space))
            {
                color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                MainMenuUI.instance.colorUI.color = color;
                playerColor.color = color;
                gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);         
            }
        }
        
        // change color over network
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameObject.GetPhotonView().RPC("changeColor", RpcTarget.AllBuffered, color.r, color.g, color.b);
        }      */  
    }

    /*[PunRPC]
    void changeColor(float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        Camera.main.backgroundColor = c;
    }*/

    [PunRPC]
    void playerColorChange(float r, float g, float b)
    {
        playerColor = gameObject.GetComponent<MeshRenderer>().material;
        Color c = new Color(r, g, b);
        playerColor.color = c;
    }
}
