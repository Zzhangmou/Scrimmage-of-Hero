using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class AltasPacker : EditorWindow
{
    // ָ��Ҫ�����ļ���
    private string[] checkDirs = new string[1];

    private string rootPath = "Assets/ArtRes/PT/UI/";
    private string spriteAtlasPath = "Assets/ArtRes/SpriteAtlas/";
    private string defaultName = "Default";

    [MenuItem("Tools/���ͼ��")]
    public static void ShowAltasPacker()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(AltasPacker));
        window.titleContent = new GUIContent("AltasPacker");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("StartPackAll"))
        {
            checkDirs[0] = rootPath;
            StartPack();
        }
    }

    private void StartPack()
    {
        Debug.Log("Start pack");
        Dictionary<string, List<string>> texturesDic = new Dictionary<string, List<string>>();
        GetPackTextureDic(texturesDic);
        SaveAltasTexture(texturesDic);
        Debug.Log("pack complete");
    }

    /// <summary>
    /// ���������ȡ����Ҫ����������������Ƿ���洢���ֵ���
    /// </summary>
    /// <param name="dic"></param>
    private void GetPackTextureDic(Dictionary<string, List<string>> dic)
    {
        string[] guides = AssetDatabase.FindAssets("t:Sprite", checkDirs);
        foreach (var guid in guides)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            //Debug.Log(path);

            //���·��
            string relativePath = path.Substring(rootPath.Length);
            int index = relativePath.IndexOf('/');

            List<string> list;
            if (index > 0)
            {
                // ��ȡ�ֵ��
                string key = relativePath.Substring(0, index);
                // ��ӵ��ֵ���
                if (!dic.TryGetValue(key, out list))
                {
                    list = new List<string>();
                    dic.Add(key, list);
                }
            }
            else
            {
                if (!dic.TryGetValue(defaultName, out list))
                {
                    list = new List<string>();
                    dic.Add(defaultName, list);
                }
            }
            list.Add(path);
        }
    }
    private void SaveAltasTexture(Dictionary<string, List<string>> dic)
    {
        string dir = Path.Combine(Application.dataPath, "../ArtRes/SpriteAtlas");

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        foreach (var item in dic)
        {
            List<string> path_list = item.Value;
            // ���ͼ��������
            try
            {
                if (path_list.Count > 1) // ����1�Ŵ�ͼ��
                {
                    SpriteAtlas spriteAtlas = new SpriteAtlas();

                    //ͼ����������
                    SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
                    {
                        blockOffset = 1,
                        enableRotation = false,
                        enableTightPacking = false,
                        padding = 8,
                    };

                    spriteAtlas.SetPackingSettings(packSetting);
                    // ͼ����������
                    SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
                    {
                        readable = false,
                        generateMipMaps = false,
                        sRGB = true,
                        filterMode = FilterMode.Bilinear,
                    };

                    spriteAtlas.SetTextureSettings(textureSettings);
                    AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath + item.Key + ".spriteatlas");
                    // ��AB����ǩ
                    AssetImporter importer = AssetImporter.GetAtPath(spriteAtlasPath + item.Key + ".spriteatlas");
                    importer.assetBundleName = "ui";

                    //���sprite
                    Sprite[] sprites = new Sprite[path_list.Count];
                    for (int i = 0; i < path_list.Count; i++)
                    {
                        sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path_list[i]);
                    }
                    spriteAtlas.Add(sprites);

                    EditorUtility.SetDirty(spriteAtlas);
                    AssetDatabase.SaveAssets();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        AssetDatabase.Refresh();
    }
}
