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
    public float jumpSpeed = 1f;
    public CharacterController characterController;
    public Material playerColor;
    float horizontal;
    float vertical;
    float velocity;
    CameraManagaer cam;
    Vector3 camDir;


    AudioSource source;
    public AudioClip jump;
    public AudioClip colorChange;
    public AudioClip fun;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        characterController = gameObject.GetComponent<CharacterController>();
        playerColor = gameObject.GetComponent<MeshRenderer>().material;
        cam = gameObject.GetComponent<CameraManagaer>();

        if (gameObject.GetPhotonView().IsMine)
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            MainMenuUI.instance.colorUI.color = color;
            playerColor.color = color;
            gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);
        }
    }

    private void Update()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            //Debug.Log("grounded: " + characterController.isGrounded + " velocity: " + velocity);
            horizontal = Input.GetAxis("Horizontal") * speed;
            vertical = Input.GetAxis("Vertical") * speed;

            if (characterController.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    //jump
                    velocity = jumpSpeed;
                }
                else
                {
                    velocity = 0;
                }
                
            }
            velocity += Physics.gravity.y * Time.deltaTime;
            //characterController.Move( * Time.deltaTime);

            camDir = cam.GetCameraDirection();
            if (cam != null)
            {
                cam.OnStartFollowing();
            }

            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, camDir.y, gameObject.transform.eulerAngles.z);
            characterController.Move((gameObject.transform.right * horizontal + gameObject.transform.forward * vertical + (new Vector3(0, 1, 0) * velocity)) * Time.deltaTime);            

            // changes local user's color to apply
            if (Input.GetKeyDown(KeyCode.E))
            {
                //colorchange
                color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                MainMenuUI.instance.colorUI.color = color;
                playerColor.color = color;
                gameObject.GetPhotonView().RPC("playerColorChange", RpcTarget.AllBufferedViaServer, color.r, color.g, color.b);         
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //fun
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
