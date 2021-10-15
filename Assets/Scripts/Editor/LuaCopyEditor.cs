using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ns
{
    /// <summary>
    /// 
    /// </summary>
    public class LuaCopyEditor : Editor
    {
        [MenuItem("XLua/CopyLuaToTxt")]
        public static void CopyLuaToTxt()
        {
            //�ҵ�����Lua�ļ�
            string path = Application.dataPath + "/Resources/Lua/";
            //�ж�·���Ƿ����
            if (!Directory.Exists(path))
                return;

            string[] allPath = Directory.GetFiles(path, "*.lua");

            //��Lua�ļ��������µ��ļ���
            string newPath = Application.dataPath + "/LuaTxt/";//������·��
            //����ǰ�����
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            else
            {
                //�õ���׺.txt ɾ��
                string[] oldPath = Directory.GetFiles(newPath, "*.txt");
                foreach (var item in oldPath)
                {
                    File.Delete(item);
                }
            }

            List<string> newFileNames = new List<string>();
            string fileName;
            for (int i = 0; i < allPath.Length; i++)
            {
                fileName = newPath + allPath[i].Substring(allPath[i].LastIndexOf("/") + 1) + ".txt";
                newFileNames.Add(fileName);
                File.Copy(allPath[i], fileName);
            }
            //ˢ��
            AssetDatabase.Refresh();

            //�޸�Ab��ǩ
            for (int i = 0; i < newFileNames.Count; i++)
            {
                //����·��������Asset�ļ��е�
                AssetImporter importer = AssetImporter.GetAtPath(newFileNames[i].Substring(newFileNames[i].IndexOf("Assets")));
                if (importer != null)
                    importer.assetBundleName = "lua";
            }
        }
    }
}
