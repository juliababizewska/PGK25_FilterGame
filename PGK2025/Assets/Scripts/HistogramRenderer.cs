using UnityEngine;

public static class HistogramRenderer
{

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

        Color bg = new Color(0f, 0f, 0f, 0f); 
        Color[] px = new Color[width * height];
        for (int i = 0; i < px.Length; i++) px[i] = bg;

        float binsPerColumn = (float)bins / width;

        for (int x = 0; x < width; x++)
        {
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


            int rHeight = Mathf.RoundToInt(rVal * (height - 2));
            int gHeight = Mathf.RoundToInt(gVal * (height - 2));
            int bHeight = Mathf.RoundToInt(bVal * (height - 2));

            int xIndex = x;
            for (int y = 0; y < bHeight; y++)
            {
                int idx = (y * width) + xIndex;
                px[idx] = Color.blue;
            }
            for (int y = 0; y < gHeight; y++)
            {
                int idx = (y * width) + xIndex;
                px[idx] = Color.Lerp(px[idx], Color.green, 0.7f);
            }
            for (int y = 0; y < rHeight; y++)
            {
                int idx = (y * width) + xIndex;
                px[idx] = Color.Lerp(px[idx], Color.red, 0.7f);
            }
        }
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }
}
  