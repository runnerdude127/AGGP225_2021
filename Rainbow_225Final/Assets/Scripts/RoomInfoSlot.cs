using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RoomInfoSlot : InfoSlot
{
    public TMP_Text slotDetail;
    Gamemode roomGamemode;
    public string gamemodeName;

    public override void Awake()
    {
        base.Awake();
        RectTransform pl = LobbyManager.instance.roomlist;
        this.transform.SetParent(pl);

        /*if (gameObject.GetPhotonView().IsMine)
        {
            infoSet(LobbyManager.instance.myRoomInfo.Name, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            roomExtras(gamemode, LobbyManager.instance.myRoomInfo.PlayerCount, LobbyManager.instance.myRoomInfo.MaxPlayers);
        }*/
    }

    public void roomClick()
    {
        LobbyManager.instance.roomSelected = gameObject.GetComponent<RoomInfoSlot>();
        //LobbyManager.instance.joinRoom(slotName.text);
    }

    public override void infoSet(string name)
    {
        base.infoSet(name);
        //roomExtras(PhotonManager.instance.GetRoomSize(name), detail);
    }

    public void roomExtras(int gamemodeInfo, int players, int roomMax)
    {
        slotDetail.text = players.ToString() + "/" + roomMax.ToString();
        roomGamemode = PhotonManager.instance.gamemodeList[gamemodeInfo];
        slotIcon.sprite = roomGamemode.icon;
        gamemodeName = roomGamemode.name;
    }
}
