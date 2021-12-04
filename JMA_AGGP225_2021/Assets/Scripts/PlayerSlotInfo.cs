using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PlayerSlotInfo : InfoSlot
{
    bool ready = false;

    public Sprite PlayerIcon;
    public Sprite HostIcon;
    public Sprite ReadyIcon;

    public override void Awake()
    {
        base.Awake();
        RectTransform pl = LobbyManager.instance.playerlist;
        this.transform.SetParent(pl);

        if (gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetPhotonView().RPC("playerSlotUpdate", RpcTarget.AllBufferedViaServer, PhotonManager.instance.myUsername, PhotonNetwork.IsMasterClient, PhotonManager.instance.myColor.r, PhotonManager.instance.myColor.g, PhotonManager.instance.myColor.b);
        }
    }

    public override void infoSet(string name, Color bgColor)
    {
        base.infoSet(name, bgColor);
    }

    [PunRPC]
    void playerSlotUpdate(string nameSet, bool isHost, float r, float g, float b)
    {
        slotBG.color = new Color(r, g, b);
        slotName.text = nameSet;

        if (ready == false)
        {
            if (isHost == true)
            {
                slotIcon.sprite = HostIcon;
            }
            else
            {
                slotIcon.sprite = PlayerIcon;
            }
        }
        else
        {
            slotIcon.sprite = ReadyIcon;
        }
    }
}
