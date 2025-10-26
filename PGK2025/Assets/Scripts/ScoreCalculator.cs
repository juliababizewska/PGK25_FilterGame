using UnityEngine;
using System;

public static class ScoreCalculator
{
    // MAE-based score: 1 - normalized MAE (kanały RGB)
    public static float ComputeMatchScore(Color[] a, Color[] b)
    {
        if (a == null || b == null || a.Length != b.Length) return 0f;
        double sumAbs = 0.0;
        for (int i = 0; i < a.Length; i++)
        {
            sumAbs += Math.Abs(a[i].r - b[i].r);
            sumAbs += Math.Abs(a[i].g - b[i].g);
            sumAbs += Math.Abs(a[i].b - b[i].b);
        }
        double mae = sumAbs / (a.Length * 3.0);
        double score = 1.0 - mae;
        return Mathf.Clamp01((float)score);
    }
}
