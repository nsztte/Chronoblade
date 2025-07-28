using UnityEngine;
using UnityEditor;
using System.IO;

public class InvertRoughnessMapEditor : EditorWindow
{
    private Texture2D roughnessTexture;
    private string savePath = "Assets/_Project/Editor/"; // 저장 경로
    private string saveName = "Generated_SmoothnessMap.png"; // 저장 파일명

    [MenuItem("Tools/Texture Tools/Invert Roughness Map")]
    public static void ShowWindow()
    {
        GetWindow<InvertRoughnessMapEditor>("Invert Roughness Map");
    }

    private void OnGUI()
    {
        GUILayout.Label("Roughness → Smoothness 변환", EditorStyles.boldLabel);

        roughnessTexture = (Texture2D)EditorGUILayout.ObjectField("Roughness Texture", roughnessTexture, typeof(Texture2D), false);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Generate Smoothness Map"))
        {
            if (roughnessTexture == null)
            {
                Debug.LogError("Roughness 텍스처를 먼저 지정해주세요.");
                return;
            }

            GenerateSmoothnessMap();
        }
    }

    private void GenerateSmoothnessMap()
    {
        string path = AssetDatabase.GetAssetPath(roughnessTexture);
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
        if (!importer.isReadable)
        {
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        Texture2D source = roughnessTexture;
        Texture2D result = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);

        for (int y = 0; y < source.height; y++)
        {
            for (int x = 0; x < source.width; x++)
            {
                Color pixel = source.GetPixel(x, y);
                float inverted = 1f - pixel.r;
                result.SetPixel(x, y, new Color(inverted, inverted, inverted, 1f));
            }
        }

        result.Apply();

        byte[] pngData = result.EncodeToPNG();
        saveName = roughnessTexture.name + "_SmoothnessMap.png";
        string fullPath = Path.Combine(savePath, saveName);
        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("덮어쓰기 확인", "같은 이름의 파일이 이미 존재합니다. 덮어쓸까요?", "예", "아니오"))
            {
                return;
            }
        }
        File.WriteAllBytes(fullPath, pngData);

        AssetDatabase.Refresh();
        Debug.Log("Smoothness Map 생성 완료: " + fullPath);
    }
}
