using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public Color color;
    
    private void Start()
    {       
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void Update()
    {
        // change color over network
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetPhotonView().RPC("changeColor", RpcTarget.AllBuffered, color.r, color.g, color.b);
        }

        // changes local user's color to apply
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            MainMenuUI.instance.colorUI.color = color;
        }
    }

    [PunRPC]
    void changeColor(float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        Camera.main.backgroundColor = c;
    }
}
