using UnityEngine;
using UnityEngine.UI;

public class SliderButtons : MonoBehaviour
{
    public Slider slider;
    public float step = 0.01f;

    public void Increase()
    {
        slider.value = Mathf.Min(slider.maxValue, slider.value + step);
    }

    public void Decrease()
    {
        slider.value = Mathf.Max(slider.minValue, slider.value - step);
    }
}
