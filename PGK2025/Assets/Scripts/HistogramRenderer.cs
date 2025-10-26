using UnityEngine;

/// <summary>
/// Rysuje histogramy (RGB) do texture2D. Prosta wizualizacja: pionowe słupki.
/// </summary>
public static class HistogramRenderer
{
    /// <summary>
    /// Renders histograms (float[3][bins]) to a Texture2D.
    /// height - pixel height of the resulting texture.
    /// If bins > width, bins will be aggregated.
    /// </summary>
    public static Texture2D RenderRGBHistogram(float[][] histograms, int width = 256, int height = 100)
    {
        if (histograms == null || histograms.Length != 3) 
        {
            // return empty texture
            Texture2D empty = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] clearPixels = new Color[width * height];
            for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = Color.clear;
            empty.SetPixels(clearPixels);
            empty.Apply();
            return empty;
        }

        int bins = histograms[0].Length;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // prepare background (transparent or dark)
        Color bg = new Color(0f, 0f, 0f, 0f); // transparent
        Color[] px = new Color[width * height];
        for (int i = 0; i < px.Length; i++) px[i] = bg;

        // aggregation factor: how many bins per pixel column
        float binsPerColumn = (float)bins / width;

        for (int x = 0; x < width; x++)
        {
            // aggregate histogram values for this column
            int startBin = Mathf.FloorToInt(x * binsPerColumn);
            int endBin = Mathf.Min(bins - 1, Mathf.FloorToInt((x + 1) * binsPerColumn));
            if (endBin < startBin) endBin = startBin;
            float rVal = 0f, gVal = 0f, bVal = 0f;
            for (int b = startBin; b <= endBin; b++)
            {
                rVal += histograms[0][b];
                gVal += histograms[1][b];
                bVal += histograms[2][b];
            }
            float denom = (endBin - startBin + 1);
            rVal /= denom;
            gVal /= denom;
            bVal /= denom;

            // compute final intensity per channel (already normalized)
            // draw vertical bar height for each channel (we'll stack them slightly so they are visible)
            int rHeight = Mathf.RoundToInt(rVal * (height - 2));
            int gHeight = Mathf.RoundToInt(gVal * (height - 2));
            int bHeight = Mathf.RoundToInt(bVal * (height - 2));

            // draw blue at bottom, then green above, then red on top (or overlay)
            int xIndex = x;
            for (int y = 0; y < bHeight; y++)
            {
                int idx = (y * width) + xIndex;
                px[idx] = Color.blue;
            }
            for (int y = 0; y < gHeight; y++)
            {
                int idx = (y * width) + xIndex;
                // blend green over blue
                px[idx] = Color.Lerp(px[idx], Color.green, 0.7f);
            }
            for (int y = 0; y < rHeight; y++)
            {
                int idx = (y * width) + xIndex;
                px[idx] = Color.Lerp(px[idx], Color.red, 0.7f);
            }
        }

        // Flip vertical because we drew from bottom (y=0) upward but SetPixels expects row-major from bottom-left? 
        // We'll write as is and then apply, but it's okay for simple display.
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }
}
  