using UnityEngine;

public class WeaponIconCapture : MonoBehaviour
{
    public Camera captureCamera;
    public string outputPath = "Assets/WeaponIcons/weapon_icon.png";
    public int resolution = 512;

    [ContextMenu("Capture Icon")]
    void Capture()
    {
        var rt = new RenderTexture(resolution, resolution, 24);
        captureCamera.targetTexture = rt;

        var tex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        captureCamera.Render();
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        tex.Apply();

        System.IO.File.WriteAllBytes(outputPath, tex.EncodeToPNG());

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        Debug.Log("무기 아이콘 캡처 완료: " + outputPath);
    }
}
