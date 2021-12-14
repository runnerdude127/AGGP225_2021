using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoSlot : MonoBehaviour
{
    public Image slotIcon;
    public TMP_Text slotName;

    public virtual void Awake()
    {

    }

    public virtual void infoSet(string name)
    {
        slotName.text = name;
    }
}
