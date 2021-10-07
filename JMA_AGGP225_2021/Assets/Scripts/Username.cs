using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class Username : MonoBehaviour
{
    public TMP_Text usernameText;

    public static Username instance; // SINGLETON INSTANCE

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        usernameText.text = PhotonManager.instance.myUsername + ": ";
        //PhotonManager.instance.gameObject.GetPhotonView().RPC("UsernameRPC", RpcTarget.AllBuffered, PhotonManager.instance.myUsername.ToString(), "hello world");
    }
}
