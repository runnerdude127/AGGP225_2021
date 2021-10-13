using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    #region InRoom UI
    public Image colorUI;
    public Meter thisPlayerHealth;
    public Image healthColor;
    public Image infectColor;
    public Image chargeColor;

    public Meter thisPlayerCharge;

    public Animator colorBlockAnim;
    Animation colorchange;
    #endregion

    public static PlayerGUI instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        colorBlockAnim = colorUI.GetComponent<Animator>();
    }

    #region UI Updates
    private void Update()
    {

    }
    #endregion
}
