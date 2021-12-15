using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Meter : MonoBehaviour
{
    public Slider slider;
    public Slider hitSlider;
    public TMP_Text healthTXT;

    public Image mainColor;
    public Image lossColor;
    public Image slideColor;

    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetMeter(float input)
    {
        healthTXT.text = input.ToString();
        slider.value = input;
        hitSlider.value = input;
    }

    public void SetCurrent(float input)
    {       
        float currentValue = hitSlider.value;
        slider.value = input;
        healthTXT.text = input.ToString();

        //Debug.Log("Current value " + currentValue + " moving to " + input);
        hitSlider.value = Mathf.MoveTowards(currentValue, input, ((currentValue - input) * 2) * Time.deltaTime);
    }

    public void SetMax(float input)
    {
        slider.maxValue = input;
        hitSlider.maxValue = input;
        slider.value = input;
        hitSlider.value = input;
        healthTXT.text = input.ToString();
    }

    public void SetMainColor(Color c)
    {
        mainColor.color = c;
    }

    public void SetLossColor(Color c)
    {
        lossColor.color = c;
    }

    public void SetSlideColor(Color c)
    {
        slideColor.color = c;
    }
}
