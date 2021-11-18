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
    public override void Awake()
    {
        base.Awake();
        RectTransform pl = LobbyManager.instance.playerlist;
        this.transform.SetParent(pl);

        if (gameObject.GetPhotonView().IsMine)
        {
            infoSet(PhotonManager.instance.myUsername, PhotonManager.instance.myColor);
        }
    }

    public override void infoSet(string name, Color bgColor)
    {
        base.infoSet(name, bgColor);
    }
}
