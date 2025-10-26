using UnityEngine;
using System;

public static class ImageProcessor
{
    // Stosuje filtry punktowe do tablicy pikseli i zwraca nową tablicę
    public static Color[] ApplyFilters(Color[] srcPixels, FilterParams p)
    {
        if (srcPixels == null) return null;
        Color[] dst = new Color[srcPixels.Length];
        for (int i = 0; i < srcPixels.Length; i++)
        {
            Color col = srcPixels[i];

            // brightness
            col.r = Mathf.Clamp01(col.r + p.brightness);
            col.g = Mathf.Clamp01(col.g + p.brightness);
            col.b = Mathf.Clamp01(col.b + p.brightness);

            // contrast (around 0.5)
            col.r = Mathf.Clamp01((col.r - 0.5f) * p.contrast + 0.5f);
            col.g = Mathf.Clamp01((col.g - 0.5f) * p.contrast + 0.5f);
            col.b = Mathf.Clamp01((col.b - 0.5f) * p.contrast + 0.5f);

            // gamma
            col.r = Mathf.Clamp01(Mathf.Pow(col.r, 1.0f / p.gamma));
            col.g = Mathf.Clamp01(Mathf.Pow(col.g, 1.0f / p.gamma));
            col.b = Mathf.Clamp01(Mathf.Pow(col.b, 1.0f / p.gamma));

            // saturation (RGB -> HSV -> mod S -> back)
            Color.RGBToHSV(col, out float H, out float S, out float V);
            S = Mathf.Clamp01(S * p.saturation);
            Color outCol = Color.HSVToRGB(H, S, V);
            outCol.a = col.a;

            dst[i] = outCol;
        }
        return dst;
    }
}
