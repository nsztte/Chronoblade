using UnityEngine;
using UnityEditor;
using System.IO;

public class MetallicSmoothnessCombiner : EditorWindow
{
    Texture2D metallicMap;
    Texture2D roughnessMap;

    [MenuItem("Tools/Texture Tools/Metallic Smoothness Combiner")]
    public static void ShowWindow()
    {
        GetWindow<MetallicSmoothnessCombiner>("Metallic Smoothness Combiner");
    }

    void OnGUI()
    {
        GUILayout.Label("Combine Metallic(R) + Roughness(A)", EditorStyles.boldLabel);
        metallicMap = (Texture2D)EditorGUILayout.ObjectField("Metallic Map (R)", metallicMap, typeof(Texture2D), false);
        roughnessMap = (Texture2D)EditorGUILayout.ObjectField("Roughness Map", roughnessMap, typeof(Texture2D), false);

        if (GUILayout.Button("Combine and Save"))
        {
            if (metallicMap == null || roughnessMap == null)
            {
                EditorUtility.DisplayDialog("Error", "Both textures must be assigned.", "OK");
                return;
            }

            string path = EditorUtility.SaveFilePanel("Save Combined Texture", "Assets/_Project/Art/Model", "_MetallicSmoothness", "png");
            if (string.IsNullOrEmpty(path)) return;

            Texture2D combined = CombineTextures(metallicMap, roughnessMap);
            byte[] pngData = combined.EncodeToPNG();

            if (path.StartsWith(Application.dataPath))
            {
                string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                File.WriteAllBytes(path, pngData);
                AssetDatabase.Refresh();

                TextureImporter importer = AssetImporter.GetAtPath(relativePath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Default;
                    importer.alphaSource = TextureImporterAlphaSource.FromInput;
                    importer.SaveAndReimport();
                }

                Debug.Log("Saved combined texture to " + relativePath);
            }
        }
    }

    Texture2D CombineTextures(Texture2D metallic, Texture2D roughness)
    {
        int width = metallic.width;
        int height = metallic.height;

        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color m = metallic.GetPixel(x, y);
                Color r = roughness.GetPixel(x, y);
                float smoothness = 1f - r.r;

                result.SetPixel(x, y, new Color(m.r, 0f, 0f, smoothness));
            }
        }

        result.Apply();
        return result;
    }
}
