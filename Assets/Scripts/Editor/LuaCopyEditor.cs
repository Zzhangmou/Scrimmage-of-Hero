using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class LuaCopyEditor : Editor
{
    [MenuItem("Tools/CopyLuaToTxt")]
    public static void CopyLuaToTxt()
    {
        //找到所有Lua文件
        string path = Application.dataPath + "/ArtRes/Lua/";
        //判断路径是否存在
        if (!Directory.Exists(path))
            return;

        string[] allPath = Directory.GetFiles(path, "*.lua", SearchOption.AllDirectories);

        //将Lua文件拷贝到新的文件夹
        string newPath = Application.dataPath + "/LuaTxt/";//定义新路径

        //拷贝前先清空
        if (Directory.Exists(newPath))
        {
            Directory.Delete(newPath, true);
        }

        Directory.CreateDirectory(newPath);

        List<string> newFileNames = new List<string>();
        string fileName;
        foreach (string luaFile in allPath)
        {
            // 计算新文件的相对路径，并创建新目录
            string relativePath = luaFile.Substring(path.Length);

            string newFileDirectory = Path.Combine(newPath, Path.GetDirectoryName(relativePath));

            if (!Directory.Exists(newFileDirectory))
                Directory.CreateDirectory(newFileDirectory);

            // 创建新的文件路径并添加 .txt 后缀
            fileName = Path.Combine(newFileDirectory, Path.GetFileNameWithoutExtension(luaFile) + ".lua.txt");
            newFileNames.Add(fileName);

            // 复制文件到新的路径
            File.Copy(luaFile, fileName);
        }
        // 修改AB标签
        string abName;
        string replacePath = "Assets/LuaTxt";
        foreach (string newFile in newFileNames)
        {
            // 获取相对于 Assets 的路径
            string assetPath = "Assets" + newFile.Substring(Application.dataPath.Length);
            string namePath = Path.GetDirectoryName(assetPath);
            abName = namePath.Length == replacePath.Length ? "lua" : namePath.Substring(replacePath.Length + 1).Replace('\\', '_');
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                importer.assetBundleName = abName;
            }
        }
        //刷新
        AssetDatabase.Refresh();
    }
}