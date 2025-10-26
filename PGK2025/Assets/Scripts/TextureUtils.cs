using UnityEngine;

public static class TextureUtils
{
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
