using System.IO;
using UnityEditor;

namespace ns
{
    /// <summary>
    /// ��Դӳ������
    /// </summary>
    public class GenerateResConfig : Editor
    {
        [MenuItem("Tools/Resources/Generate ResConfig File")]
        public static void Generate()
        {
            //������Դ�����ļ�
            //1.���� Resources Ŀ¼������Ԥ�Ƽ�����·��
            string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
            for (int i = 0; i < resFiles.Length; i++)
            {
                resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
                //2.���ɶ�Ӧ��ϵ
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
                resFiles[i] = fileName + "=" + filePath;
            }
            //3.д���ļ�
            File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", resFiles);
            //ˢ��
            AssetDatabase.Refresh();
        }
    }
}

