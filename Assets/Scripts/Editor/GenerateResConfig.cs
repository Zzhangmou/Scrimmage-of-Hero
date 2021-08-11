using System.IO;
using UnityEditor;

namespace ns
{
    /// <summary>
    /// 资源映射生成
    /// </summary>
    public class GenerateResConfig : Editor
    {
        [MenuItem("Tools/Resources/Generate ResConfig File")]
        public static void Generate()
        {
            //生成资源配置文件
            //1.查找 Resources 目录下所有预制件完整路径
            string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
            for (int i = 0; i < resFiles.Length; i++)
            {
                resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
                //2.生成对应关系
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
                resFiles[i] = fileName + "=" + filePath;
            }
            //3.写入文件
            File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", resFiles);
            //刷新
            AssetDatabase.Refresh();
        }
    }
}

