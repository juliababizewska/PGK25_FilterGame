using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIBindings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider gammaSlider;
    public Slider saturationSlider;

    [Header("Buttons")]
    public Button randomizeButton;
    public Button checkButton;

    // exposes current params
    public FilterParams currentParams = new FilterParams();

    // events other systems can subscribe to
    public event Action OnParamsChanged;
    public event Action OnRandomizePressed;
    public event Action OnCheckPressed;

    void Start()
    {
        // set slider ranges (ensure consistent ranges)
        brightnessSlider.minValue = -0.5f; brightnessSlider.maxValue = 0.5f; brightnessSlider.value = currentParams.brightness;
        contrastSlider.minValue = 0.5f; contrastSlider.maxValue = 1.5f; contrastSlider.value = currentParams.contrast;
        gammaSlider.minValue = 0.5f; gammaSlider.maxValue = 2.5f; gammaSlider.value = currentParams.gamma;
        saturationSlider.minValue = 0f; saturationSlider.maxValue = 2f; saturationSlider.value = currentParams.saturation;

        // listeners
        brightnessSlider.onValueChanged.AddListener((v) => { currentParams.brightness = v; ParamsChanged(); });
        contrastSlider.onValueChanged.AddListener((v) => { currentParams.contrast = v; ParamsChanged(); });
        gammaSlider.onValueChanged.AddListener((v) => { currentParams.gamma = v; ParamsChanged(); });
        saturationSlider.onValueChanged.AddListener((v) => { currentParams.saturation = v; ParamsChanged(); });

        if (randomizeButton != null) randomizeButton.onClick.AddListener(() => { OnRandomizePressed?.Invoke(); });
        if (checkButton != null) checkButton.onClick.AddListener(() => { OnCheckPressed?.Invoke(); });
    }

    private void ParamsChanged()
    {
        OnParamsChanged?.Invoke();
    }

    // optional helper to set UI sliders to given params
    public void SetUIFromParams(FilterParams p)
    {
        brightnessSlider.SetValueWithoutNotify(p.brightness);
        contrastSlider.SetValueWithoutNotify(p.contrast);
        gammaSlider.SetValueWithoutNotify(p.gamma);
        saturationSlider.SetValueWithoutNotify(p.saturation);
        currentParams = p.Clone();
        OnParamsChanged?.Invoke();
    }
}
