using UnityEngine;

public static class ImageManager
{
    private static Texture2D[] loadedImages;

    /// <summary>
    /// Ładuje wszystkie obrazki z folderu Resources/Images/
    /// (muszą być tam pliki PNG/JPG z ustawionym "Read/Write Enabled = true")
    /// </summary>
    public static void Initialize()
    {
        if (loadedImages == null)
        {
            loadedImages = Resources.LoadAll<Texture2D>("Images");
            if (loadedImages.Length == 0)
                Debug.LogWarning("⚠️ Brak obrazków w Resources/Images/");
            else
                Debug.Log($"✅ Załadowano {loadedImages.Length} obrazków z Resources/Images/");
        }
    }

    /// <summary>
    /// Zwraca losowy obrazek z załadowanych
    /// </summary>
    public static Texture2D GetRandomImage()
    {
        if (loadedImages == null || loadedImages.Length == 0)
        {
            Initialize();
            if (loadedImages == null || loadedImages.Length == 0)
                return null;
        }

        int index = Random.Range(0, loadedImages.Length);
        return loadedImages[index];
    }
}
