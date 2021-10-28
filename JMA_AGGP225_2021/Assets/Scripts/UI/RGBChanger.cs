using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBChanger : MonoBehaviour
{
    public Image changer;
    public Material matChanger;
    public Color currentColor;

    public Slider r;
    public Slider g;
    public Slider b;

    private void Start()
    {
        r.value = PhotonManager.instance.myColor.r;
        g.value = PhotonManager.instance.myColor.g;
        b.value = PhotonManager.instance.myColor.b;
    }

    void Update()
    {
        currentColor = new Color(r.value, g.value, b.value);
        changer.color = currentColor;
        if (matChanger)
        {
            matChanger.color = currentColor;
        }
    }
}
