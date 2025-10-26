using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UIBindings))]
public class GameController : MonoBehaviour
{
    [Header("UI & Images")]
    public RawImage leftRawImage;
    public RawImage rightRawImage;
    public TMP_Text scoreText;
    public TMP_Text resultText;

    [Header("Source")]
    public Texture2D originalTexture;

    [Header("Processing")]
    public int procWidth = 320;


    [Header("Histograms UI (optional)")]
    public RawImage leftHistogramImage;
    public RawImage rightHistogramImage;
    public int histogramBins = 256;
    public int histogramWidth = 256;
    public int histogramHeight = 100;

    // internals
    private Texture2D leftTexture;
    private Texture2D rightTexture;
    private Color[] originalPixelsSmall;
    private int procHeight;
    private FilterParams targetParams;
    private UIBindings ui;

    void Awake()
    {
        ui = GetComponent<UIBindings>();
    }

    void Start()
    {
        // 🔹 Pobierz losowy obrazek
        ImageManager.Initialize();
        if (originalTexture == null)
            originalTexture = ImageManager.GetRandomImage();

        if (originalTexture == null)
        {
            Debug.LogError("Nie znaleziono tekstur! Upewnij się, że są w Resources/Images.");
            enabled = false;
            return;
        }

        if (originalTexture == null)
        {
            Debug.LogError("Assign originalTexture in inspector.");
            enabled = false;
            return;
        }

        float aspect = (float)originalTexture.height / originalTexture.width;
        procHeight = Mathf.RoundToInt(procWidth * aspect);

        leftTexture = new Texture2D(procWidth, procHeight, TextureFormat.RGBA32, false);
        rightTexture = new Texture2D(procWidth, procHeight, TextureFormat.RGBA32, false);

        Texture2D small = TextureUtils.ScaleTexture(originalTexture, procWidth, procHeight);
        originalPixelsSmall = small.GetPixels();

        leftRawImage.texture = leftTexture;
        rightRawImage.texture = rightTexture;

        // subscribe to UI events
        ui.OnParamsChanged += OnPlayerParamsChanged;
        ui.OnRandomizePressed += GenerateRandomTarget;
        ui.OnCheckPressed += CheckMatch;

        // initial render
        ApplyPlayerParamsToLeft();
        GenerateRandomTarget();
        UpdateScoreDisplay();
    }

    void OnDestroy()
    {
        if (ui != null)
        {
            ui.OnParamsChanged -= OnPlayerParamsChanged;
            ui.OnRandomizePressed -= GenerateRandomTarget;
            ui.OnCheckPressed -= CheckMatch;
        }
    }

    private void OnPlayerParamsChanged()
    {
        ApplyPlayerParamsToLeft();
        UpdateScoreDisplay();
    }

    private void ApplyPlayerParamsToLeft()
    {
        Color[] dst = ImageProcessor.ApplyFilters(originalPixelsSmall, ui.currentParams);
        leftTexture.SetPixels(dst);
        leftTexture.Apply();
        UpdateHistograms();

    }

    private void GenerateRandomTarget()
    {
        targetParams = new FilterParams
        {
            brightness = UnityEngine.Random.Range(-0.5f, 0.5f),
            contrast = UnityEngine.Random.Range(0.5f, 1.5f),
            gamma = UnityEngine.Random.Range(0.5f, 2.5f),
            saturation = UnityEngine.Random.Range(0f, 2f)
        };

        // prepare rightTexture
        Color[] dst = ImageProcessor.ApplyFilters(originalPixelsSmall, targetParams);
        rightTexture.SetPixels(dst);
        rightTexture.Apply();
        UpdateHistograms();

        resultText.text = "Dopasuj suwaki do prawego obrazka";
        // (opcjonalnie) reset player's UI to defaults:
        // ui.SetUIFromParams(new FilterParams()); 
    }

    private void UpdateScoreDisplay()
    {
        float score = ScoreCalculator.ComputeMatchScore(leftTexture.GetPixels(), rightTexture.GetPixels());
        if (scoreText != null) scoreText.text = $"Score: {score:F3}";
    }

    private void CheckMatch()
    {
        float score = ScoreCalculator.ComputeMatchScore(leftTexture.GetPixels(), rightTexture.GetPixels());
        if (score >= 0.95f)
            resultText.text = $"Wygrana! Score = {score:F3}";
        else
            resultText.text = $"Score = {score:F3} — próbuj dalej";
    }

    private void UpdateHistograms()
    {
        // left
        if (leftHistogramImage != null)
        {
            Color[] leftPix = leftTexture.GetPixels();
            var leftHist = HistogramCalculator.ComputeRGBHistogram(leftPix, histogramBins);
            Texture2D leftHistTex = HistogramRenderer.RenderRGBHistogram(leftHist, histogramWidth, histogramHeight);
            leftHistogramImage.texture = leftHistTex;
        }

        // right
        if (rightHistogramImage != null)
        {
            Color[] rightPix = rightTexture.GetPixels();
            var rightHist = HistogramCalculator.ComputeRGBHistogram(rightPix, histogramBins);
            Texture2D rightHistTex = HistogramRenderer.RenderRGBHistogram(rightHist, histogramWidth, histogramHeight);
            rightHistogramImage.texture = rightHistTex;
        }
    }

    public void ChangeImage()
    {
        Texture2D newTex = ImageManager.GetRandomImage();
        if (newTex == null)
        {
            Debug.LogWarning("❌ Brak dostępnych obrazków w Resources/Images/");
            return;
        }

        originalTexture = newTex;

        // zeskaluj do mniejszej wersji (jeśli masz kod scaleTexture)
        Texture2D small = TextureUtils.ScaleTexture(originalTexture, procWidth, procHeight);
        originalPixelsSmall = small.GetPixels();

        // odśwież lewy/prawy obraz
        GenerateRandomTarget();
        ApplyPlayerParamsToLeft();

        Debug.Log($"🔄 Zmieniono obrazek na: {originalTexture.name}");
    }

}
