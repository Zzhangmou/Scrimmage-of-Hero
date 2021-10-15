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
            //找到所有Lua文件
            string path = Application.dataPath + "/Resources/Lua/";
            //判断路径是否存在
            if (!Directory.Exists(path))
                return;

            string[] allPath = Directory.GetFiles(path, "*.lua");

            //将Lua文件拷贝到新的文件夹
            string newPath = Application.dataPath + "/LuaTxt/";//定义新路径
            //拷贝前先清空
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            else
            {
                //得到后缀.txt 删除
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
            //刷新
            AssetDatabase.Refresh();

            //修改Ab标签
            for (int i = 0; i < newFileNames.Count; i++)
            {
                //传入路径必须是Asset文件夹的
                AssetImporter importer = AssetImporter.GetAtPath(newFileNames[i].Substring(newFileNames[i].IndexOf("Assets")));
                if (importer != null)
                    importer.assetBundleName = "lua";
            }
        }
    }
}
