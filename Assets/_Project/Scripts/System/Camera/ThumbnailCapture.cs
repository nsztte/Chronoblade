using System.IO;
using UnityEngine;

public class ThumbnailCapture : MonoBehaviour
{
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 288;

    private Camera previewCamera;
    private RenderTexture rt;

    private void Awake()
    {
        previewCamera = GetComponent<Camera>();
        if (previewCamera == null)
        {
            Debug.LogError("[ThumbnailCapture] Camera 컴포넌트 필요");
            enabled = false;
            return;
        }

        previewCamera.enabled = false;

        // 메인카메라 설정 복사
        if (Camera.main != null && Camera.main != previewCamera)
            previewCamera.CopyFrom(Camera.main);

        // UI 레이어 제외 — Camera.main이 null일 수 있으니 체크
        int uiLayer = LayerMask.NameToLayer("UI");
        int baseMask = (Camera.main != null) ? Camera.main.cullingMask : ~0; // main 없으면 전체 마스크
        if (uiLayer >= 0) previewCamera.cullingMask = baseMask & ~(1 << uiLayer);
        else previewCamera.cullingMask = baseMask;

        rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
        {
            useMipMap = false,
            autoGenerateMips = false
        };
        rt.Create();
    }

    void OnDisable()
    {
        if (previewCamera != null && previewCamera.targetTexture != null)
        previewCamera.targetTexture = null;

        if (rt != null)
        {
            rt.Release();
            Destroy(rt);
            rt = null;
        }
    }

    public bool CaptureToFile(string fullPath)
    {
        if (previewCamera == null || rt == null) return false;

        Texture2D tex = null;
        RenderTexture prev = RenderTexture.active;
        try
        {
            previewCamera.targetTexture = rt;
            RenderTexture.active = rt;
            previewCamera.Render();

            tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            byte[] png = tex.EncodeToPNG();

            var dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(fullPath, png);

            Destroy(tex);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Thumbnail save failed: {e}");
            return false;
        }
        finally
        {
            if (tex != null) Destroy(tex);
            RenderTexture.active = prev;
            previewCamera.targetTexture = null;
        }
    }
}
