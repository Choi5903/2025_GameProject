using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingSlider : MonoBehaviour
{
    public Slider slider;
    public float speed = 1.0f;
    private bool isMoving = false;
    private bool goingUp = true;

    public void StartSlider()
    {
        isMoving = true;
        goingUp = true;
        slider.value = 0f;
    }

    public void StopSlider()
    {
        isMoving = false;
    }

    public void ResetSlider()
    {
        isMoving = false;
        slider.value = 0f;
    }

    void Update()
    {
        if (!isMoving) return;

        float delta = speed * Time.deltaTime;
        if (goingUp)
        {
            slider.value += delta;
            if (slider.value >= 1f)
            {
                slider.value = 1f;
                goingUp = false;
            }
        }
        else
        {
            slider.value -= delta;
            if (slider.value <= 0f)
            {
                slider.value = 0f;
                goingUp = true;
            }
        }
    }

    public float GetCurrentValue() => slider.value;
}
