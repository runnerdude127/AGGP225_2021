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

    public Sprite Red;
    public Sprite Orange;
    public Sprite Yellow;
    public Sprite Lime;
    public Sprite Green;
    public Sprite Cyan;
    public Sprite Blue;
    public Sprite Purple;

    public override void Awake()
    {
        base.Awake();
        RectTransform pl = LobbyManager.instance.playerlist;
        this.transform.SetParent(pl);

        if (gameObject.GetPhotonView().IsMine)
        {
            gameObject.GetPhotonView().RPC("playerSlotUpdate", RpcTarget.AllBufferedViaServer, PhotonManager.instance.myUsername, PhotonNetwork.IsMasterClient, PhotonManager.instance.classList[0]); ;
        }
    }

    public override void infoSet(string name)
    {
        base.infoSet(name);
    }

    [PunRPC]
    void playerSlotUpdate(string nameSet, string playerClass, bool isHost)
    {
        slotName.text = nameSet;

        if (playerClass == "Vermili")
        {
            slotIcon.sprite = Red;
        }
        else
        {
            slotIcon.sprite = Orange;
        }
    }
}
