using UnityEngine;
using System;

public static class HistogramCalculator
{
    public static float[][] ComputeRGBHistogram(Color[] pixels, int binsCount = 256)
    {
        if (pixels == null || pixels.Length == 0) return new float[3][] { new float[binsCount], new float[binsCount], new float[binsCount] };

        int[] r = new int[binsCount];
        int[] g = new int[binsCount];
        int[] b = new int[binsCount];

        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];
            int ri = Mathf.Clamp((int)(c.r * (binsCount - 1)), 0, binsCount - 1);
            int gi = Mathf.Clamp((int)(c.g * (binsCount - 1)), 0, binsCount - 1);
            int bi = Mathf.Clamp((int)(c.b * (binsCount - 1)), 0, binsCount - 1);
            r[ri]++;
            g[gi]++;
            b[bi]++;
        }

        float[] rf = new float[binsCount];
        float[] gf = new float[binsCount];
        float[] bf = new float[binsCount];

        int maxCount = 1;
        for (int i = 0; i < binsCount; i++)
        {
            if (r[i] > maxCount) maxCount = r[i];
            if (g[i] > maxCount) maxCount = g[i];
            if (b[i] > maxCount) maxCount = b[i];
        }

        for (int i = 0; i < binsCount; i++)
        {
            rf[i] = (float)r[i] / maxCount;
            gf[i] = (float)g[i] / maxCount;
            bf[i] = (float)b[i] / maxCount;
        }

        return new float[3][] { rf, gf, bf };
    }
}
