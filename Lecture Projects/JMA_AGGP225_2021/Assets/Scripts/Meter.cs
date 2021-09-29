using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    public Slider slider;
    public Slider hitSlider;

    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetMeter(float input)
    {
        slider.value = input;
        hitSlider.value = input;
    }

    public void SetCurrent(float input)
    {       
        float currentValue = hitSlider.value;
        slider.value = input;

        //Debug.Log("Current value " + currentValue + " moving to " + input);
        hitSlider.value = Mathf.MoveTowards(currentValue, input, ((currentValue - input) * 2) * Time.deltaTime);
    }

    public void SetMax(float input)
    {
        slider.maxValue = input;
        hitSlider.maxValue = input;
        slider.value = input;
        hitSlider.value = input;
    }
}
