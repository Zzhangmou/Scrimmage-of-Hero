using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class AltasPacker : EditorWindow
{
    // 指定要检查的文件夹
    private string[] checkDirs = new string[1];

    private string rootPath = "Assets/ArtRes/PT/UI/";
    private string spriteAtlasPath = "Assets/ArtRes/SpriteAtlas/";
    private string defaultName = "Default";

    [MenuItem("Tools/打包图集")]
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
    /// 这个方法获取所有要打包的纹理，并将它们分类存储到字典中
    /// </summary>
    /// <param name="dic"></param>
    private void GetPackTextureDic(Dictionary<string, List<string>> dic)
    {
        string[] guides = AssetDatabase.FindAssets("t:Sprite", checkDirs);
        foreach (var guid in guides)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            //Debug.Log(path);

            //相对路径
            string relativePath = path.Substring(rootPath.Length);
            int index = relativePath.IndexOf('/');

            List<string> list;
            if (index > 0)
            {
                // 截取字典键
                string key = relativePath.Substring(0, index);
                // 添加到字典中
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
            // 打包图集并保存
            try
            {
                if (path_list.Count > 1) // 大于1才打图集
                {
                    SpriteAtlas spriteAtlas = new SpriteAtlas();

                    //图集基础设置
                    SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
                    {
                        blockOffset = 1,
                        enableRotation = false,
                        enableTightPacking = false,
                        padding = 8,
                    };

                    spriteAtlas.SetPackingSettings(packSetting);
                    // 图集纹理设置
                    SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
                    {
                        readable = false,
                        generateMipMaps = false,
                        sRGB = true,
                        filterMode = FilterMode.Bilinear,
                    };

                    spriteAtlas.SetTextureSettings(textureSettings);
                    AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath + item.Key + ".spriteatlas");
                    // 打AB包标签
                    AssetImporter importer = AssetImporter.GetAtPath(spriteAtlasPath + item.Key + ".spriteatlas");
                    importer.assetBundleName = "ui";

                    //添加sprite
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
