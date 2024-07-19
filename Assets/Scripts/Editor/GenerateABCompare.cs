using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GenerateABCompare
{
    //[MenuItem("Tools/AB/Generate ABCompare File")]
    public static void CreateABCompareFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/StandaloneWindows/");
        FileInfo[] fileInfos = directory.GetFiles();

        string abCompareInfo = string.Empty;

        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == string.Empty)
            {
                abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GetMD5(fileInfo.FullName) + "|";
            }
        }

        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/StandaloneWindows/ABCompareInfo.txt", abCompareInfo);

        Debug.Log("AB���Ա��ļ����ɳɹ�");

    }
    public static string GetMD5(string filePath)
    {
        //���ļ�������ʽ��
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //�õ�MD5�� 16���ֽ� ����
            byte[] info = md5.ComputeHash(file);

            file.Close();

            //��16���ֽ�ת��Ϊ 16���� ƴ�ӳ��ַ��� ����MD5�볤��
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < info.Length; i++)
            {
                sb.Append(info[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
