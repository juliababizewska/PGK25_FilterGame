using UnityEngine;

[System.Serializable]
public class FilterParams
{
    [Header("Values")]
    public float brightness = 0f;   // [-0.5, 0.5]
    public float contrast = 1f;     // [0.5, 1.5]
    public float gamma = 1f;        // [0.5, 2.5]
    public float saturation = 1f;   // [0, 2]

    // helper: copy
    public FilterParams Clone()
    {
        return new FilterParams {
            brightness = brightness,
            contrast = contrast,
            gamma = gamma,
            saturation = saturation
        };
    }
}
