using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSlotInfo : MonoBehaviour
{
    public TMP_Text myName;
    public Image mySlotBG;

    public void infoSet(string name, Color color)
    {
        myName.text = name;
        mySlotBG.color = color;
    }
}
