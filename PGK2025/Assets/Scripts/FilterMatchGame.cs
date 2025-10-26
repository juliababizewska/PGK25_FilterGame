using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Attach to an empty GameObject called "GameController"
public class FilterMatchGame : MonoBehaviour
{
    [Header("UI")]
    public RawImage leftRawImage;    // edytowany obraz
    public RawImage rightRawImage;   // target
    public Slider brightnessSlider;  // range [-0.5, 0.5]
    public Slider contrastSlider;    // range [0.5, 1.5]
    public Slider gammaSlider;       // range [0.5, 2.5]
    public Slider saturationSlider;  // range [0, 2]
    public TMP_Text scoreText;
    public TMP_Text resultText;
    public Button randomizeButton;
    public Button checkButton;

    [Header("Source Image (assign in Inspector)")]
    public Texture2D originalTexture; // wczytaj przez Inspector (np. PNG w Assets -> Texture Type: Sprite/Default)

    // internal
    private Texture2D leftTexture;   // aktualny (po filtrach) - pokazujemy w leftRawImage
    private Texture2D rightTexture;  // target
    private Color[] originalPixelsSmall;
    private int procWidth = 320;     // u¿ywamy skalowanej kopii do szybkich obliczeñ
    private int procHeight;

    // target parametry (do pokazania/pamieci)
    private float targetBrightness, targetContrast, targetGamma, targetSaturation;

    void Start()
    {
        if (originalTexture == null)
        {
            Debug.LogError("Przypisz originalTexture w Inspectorze.");
            enabled = false;
            return;
        }

        // ustaw zakresy suwaków
        brightnessSlider.minValue = -0.5f; brightnessSlider.maxValue = 0.5f; brightnessSlider.value = 0f;
        contrastSlider.minValue = 0.5f; contrastSlider.maxValue = 1.5f; contrastSlider.value = 1f;
        gammaSlider.minValue = 0.5f; gammaSlider.maxValue = 2.5f; gammaSlider.value = 1f;
        saturationSlider.minValue = 0f; saturationSlider.maxValue = 2f; saturationSlider.value = 1f;

        // hook events
        brightnessSlider.onValueChanged.AddListener(_ => OnParamsChanged());
        contrastSlider.onValueChanged.AddListener(_ => OnParamsChanged());
        gammaSlider.onValueChanged.AddListener(_ => OnParamsChanged());
        saturationSlider.onValueChanged.AddListener(_ => OnParamsChanged());
        randomizeButton.onClick.AddListener(GenerateRandomTarget);
        checkButton.onClick.AddListener(CheckMatch);

        // przygotuj tekstury (skaling dla wydajnoœci)
        float aspect = (float)originalTexture.height / originalTexture.width;
        procHeight = Mathf.RoundToInt(procWidth * aspect);

        leftTexture = new Texture2D(procWidth, procHeight, TextureFormat.RGBA32, false);
        rightTexture = new Texture2D(procWidth, procHeight, TextureFormat.RGBA32, false);

        // wczytaj pomniejszon¹/wstêpn¹ wersjê orygina³u
        Texture2D small = ScaleTexture(originalTexture, procWidth, procHeight);
        originalPixelsSmall = small.GetPixels();

        // inicjalne wyœwietlanie (left = orygina³)
        leftRawImage.texture = leftTexture;
        rightRawImage.texture = rightTexture;

        // ustaw prawy target losowo
        GenerateRandomTarget();
    }

    void OnParamsChanged()
    {
        // przy ka¿dej zmianie suwaka przelicz obraz
        ApplyFiltersToLeft();
        UpdateScoreDisplay();
    }

    void ApplyFiltersToLeft()
    {
        Color[] pixels = new Color[originalPixelsSmall.Length];
        Array.Copy(originalPixelsSmall, pixels, pixels.Length);

        float b = brightnessSlider.value;
        float c = contrastSlider.value;
        float g = gammaSlider.value;
        float s = saturationSlider.value;

        for (int i = 0; i < pixels.Length; i++)
        {
            Color col = pixels[i];

            // 1) brightness (dodaj)
            col.r = Mathf.Clamp01(col.r + b);
            col.g = Mathf.Clamp01(col.g + b);
            col.b = Mathf.Clamp01(col.b + b);

            // 2) contrast - prosty wokó³ 0.5
            col.r = Mathf.Clamp01((col.r - 0.5f) * c + 0.5f);
            col.g = Mathf.Clamp01((col.g - 0.5f) * c + 0.5f);
            col.b = Mathf.Clamp01((col.b - 0.5f) * c + 0.5f);

            // 3) gamma
            col.r = Mathf.Clamp01(Mathf.Pow(col.r, 1.0f / g));
            col.g = Mathf.Clamp01(Mathf.Pow(col.g, 1.0f / g));
            col.b = Mathf.Clamp01(Mathf.Pow(col.b, 1.0f / g));

            // 4) saturation - konwersja RGB -> HSV -> zmiana s -> z powrotem
            Color.RGBToHSV(col, out float H, out float S, out float V);
            S = Mathf.Clamp01(S * s);
            col = Color.HSVToRGB(H, S, V);
            col.a = pixels[i].a; // zachowaj alfa

            pixels[i] = col;
        }

        leftTexture.SetPixels(pixels);
        leftTexture.Apply();
    }

    void GenerateRandomTarget()
    {
        // losuj parametry w tych samych zakresach co suwaki
        targetBrightness = UnityEngine.Random.Range(-0.5f, 0.5f);
        targetContrast = UnityEngine.Random.Range(0.5f, 1.5f);
        targetGamma = UnityEngine.Random.Range(0.5f, 2.5f);
        targetSaturation = UnityEngine.Random.Range(0f, 2f);

        // ustaw suwaki na losowe wartoœci? Nie — gracz ma zgadn¹æ. Wiêc nie zmieniam suwaków.
        // Stwórz rightTexture na bazie parametrów target
        Color[] pixels = new Color[originalPixelsSmall.Length];
        Array.Copy(originalPixelsSmall, pixels, pixels.Length);

        for (int i = 0; i < pixels.Length; i++)
        {
            Color col = pixels[i];

            col.r = Mathf.Clamp01(col.r + targetBrightness);
            col.g = Mathf.Clamp01(col.g + targetBrightness);
            col.b = Mathf.Clamp01(col.b + targetBrightness);

            col.r = Mathf.Clamp01((col.r - 0.5f) * targetContrast + 0.5f);
            col.g = Mathf.Clamp01((col.g - 0.5f) * targetContrast + 0.5f);
            col.b = Mathf.Clamp01((col.b - 0.5f) * targetContrast + 0.5f);

            col.r = Mathf.Clamp01(Mathf.Pow(col.r, 1.0f / targetGamma));
            col.g = Mathf.Clamp01(Mathf.Pow(col.g, 1.0f / targetGamma));
            col.b = Mathf.Clamp01(Mathf.Pow(col.b, 1.0f / targetGamma));

            Color.RGBToHSV(col, out float H, out float S, out float V);
            S = Mathf.Clamp01(S * targetSaturation);
            col = Color.HSVToRGB(H, S, V);
            col.a = pixels[i].a;

            pixels[i] = col;
        }

        rightTexture.SetPixels(pixels);
        rightTexture.Apply();

        // reset result text & left to original (opcjonalnie)
        resultText.text = "Dopasuj suwaki do prawego obrazka";
    }

    void CheckMatch()
    {
        float score = ComputeMatchScore();
        UpdateScoreDisplay();
        if (score >= 0.95f)
        {
            resultText.text = $"Wygrana! Score = {score:F3}";
        }
        else
        {
            resultText.text = $"Score = {score:F3} — próbuj dalej";
        }
    }

    void UpdateScoreDisplay()
    {
        float score = ComputeMatchScore();
        scoreText.text = $"Score: {score:F3}";
    }

    // Score = 1 - normalized MAE across R,G,B (clamp 0..1)
    float ComputeMatchScore()
    {
        // pobieramy pixele z leftTexture i rightTexture
        Color[] left = leftTexture.GetPixels();
        Color[] right = rightTexture.GetPixels();
        if (left.Length != right.Length) return 0f;

        double sumAbs = 0.0;
        for (int i = 0; i < left.Length; i++)
        {
            sumAbs += Math.Abs(left[i].r - right[i].r);
            sumAbs += Math.Abs(left[i].g - right[i].g);
            sumAbs += Math.Abs(left[i].b - right[i].b);
        }
        // max mo¿liwy b³¹d to 3 * N (kana³y) * 1.0 (bo ka¿dy kana³ [0,1])
        double mae = sumAbs / (left.Length * 3.0);
        double score = 1.0 - mae; // 1 = idealnie dopasowane, 0 = maksymalny b³¹d
        return Mathf.Clamp01((float)score);
    }

    // prosta funkcja skaluj¹ca teksturê (nearest/ bilinear) - u¿ywana do obliczeñ realtime
    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D scaled = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        scaled.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        scaled.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        return scaled;
    }
}
