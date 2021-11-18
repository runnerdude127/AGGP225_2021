using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoSlot : MonoBehaviour
{
    public TMP_Text slotName;
    public Image slotBG; 

    public virtual void Awake()
    {

    }

    public virtual void infoSet(string name, Color bgColor)
    {
        slotName.text = name;
        slotBG.color = bgColor;
    }
}
