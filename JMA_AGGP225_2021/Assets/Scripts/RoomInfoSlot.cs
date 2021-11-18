using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomInfoSlot : InfoSlot
{
    public Image slotIcon;
    public TMP_Text slotDetail;

    int gamemode;
    public override void infoSet(string name, Color bgColor)
    {
        base.infoSet(name, bgColor);
        //roomExtras(PhotonManager.instance.GetRoomSize(name), detail);
    }
    public void roomExtras(int gamemodeInfo, int players, int roomMax)
    {
        slotDetail.text = players.ToString() + "/" + roomMax.ToString();
        if (gamemodeInfo == 0)
        {

        }
        else if (gamemodeInfo == 1)
        {

        }
        else if (gamemodeInfo == 2)
        {

        }
        else
        {

        }
    }
}
