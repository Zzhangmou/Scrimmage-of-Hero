using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ABTools : EditorWindow
{
    private int nowSelIndex = 0;
    private string[] targetStrings = new string[] { "PC", "IOS", "Android" };
    //��Դ������
    private string serverIP = "ftp://127.0.0.1";

    [MenuItem("Tools/AB/�򿪹��ߴ���")]
    private static void OpenWindow()
    {
        ABTools window = EditorWindow.GetWindowWithRect(typeof(ABTools), new Rect(0, 0, 350, 220)) as ABTools;
        window.Show();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 15), "ƽ̨ѡ��");
        nowSelIndex = GUI.Toolbar(new Rect(10, 30, 250, 20), nowSelIndex, targetStrings);

        GUI.Label(new Rect(10, 60, 150, 15), "��Դ��������ַ");
        serverIP = GUI.TextField(new Rect(10, 80, 150, 20), serverIP);

        if (GUI.Button(new Rect(10, 110, 100, 40), "�����Ա��ļ�"))
        {
            CreateABCompareFile();
        }

        if (GUI.Button(new Rect(115, 110, 225, 40), "����Ĭ����Դ��StreamingAssets"))
        {
            MoveABToStreamAssets();
        }

        if (GUI.Button(new Rect(10, 160, 330, 40), "�ϴ�AB���ͶԱ��ļ�"))
        {
            UploadAllABFile();
        }
    }

    private void CreateABCompareFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex]);
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
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex] + "/ABCompareInfo.txt", abCompareInfo);

        Debug.Log("AB���Ա��ļ����ɳɹ�");
        AssetDatabase.Refresh();

    }
    private string GetMD5(string filePath)
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

    private void MoveABToStreamAssets()
    {
        //ͨ���༭��Selection���еķ��� ��ȡ��Project������ѡ�е���Դ 
        Object [] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //���һ����Դ��û��ѡ�� ��û�б�Ҫ���������߼���
        if (selectedAsset.Length == 0)
            return;
        //����ƴ�ӱ���Ĭ��AB����Դ��Ϣ���ַ���
        string abCompareInfo = "";
        //����ѡ�е���Դ����
        foreach (Object asset in selectedAsset)
        {
            //ͨ��Assetdatabase�� ��ȡ ��Դ��·��
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //��ȡ·�����е��ļ��� ������Ϊ StreamingAssets�е��ļ���
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //�ж��Ƿ���.���� ����� ֤���к�׺ ������
            if (fileName.IndexOf('.') != -1)
                continue;
            //�㻹�����ڿ���֮ǰ ȥ��ȡȫ·�� Ȼ��ͨ��FIleInfoȥ��ȡ��׺���ж� �������ӵ�׼ȷ

            //����AssetDatabase�е�API ��ѡ���ļ� ���Ƶ�Ŀ��·��
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //��ȡ������StreamingAssets�ļ����е��ļ���ȫ����Ϣ
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //ƴ��AB����Ϣ���ַ�����
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GenerateABCompare.GetMD5(fileInfo.FullName);
            //��һ�����Ÿ������AB����Ϣ
            abCompareInfo += "|";
        }
        //ȥ�����һ��|���� Ϊ��֮�����ַ�������
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //������Ĭ����Դ�ĶԱ���Ϣ �����ļ�
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //ˢ�´���
        AssetDatabase.Refresh();
    }

    private void UploadAllABFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex] + '/');
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == "" || fileInfo.Extension == ".txt")
            {
                //�ϴ�
                FTPUpLoadFile(fileInfo.FullName, fileInfo.Name);
            }
        }
    }

    private async void FTPUpLoadFile(string path, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                //1.����һ��FTP���������ϴ�
                FtpWebRequest req = FtpWebRequest.Create(new Uri(serverIP + "/AB/" + targetStrings[nowSelIndex] + "/" + fileName)) as FtpWebRequest;
                //2.����һ��ͨ��ƾ֤���������ϴ�
                NetworkCredential networkCredential = new NetworkCredential("zhangmou", "maliang160");
                req.Credentials = networkCredential;
                //3.��������
                //  ���ô���Ϊnu11
                req.Proxy = null;
                //  ������Ϻ��Ƿ�رտ�������
                req.KeepAlive = false;
                //  ��������-�ϴ�
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //  ָ����������� 2����
                req.UseBinary = true;
                //4.�ϴ��ļ�
                //  ftp��������
                Stream upLoadStream = req.GetRequestStream();
                //  ��ȡ�ļ���Ϣ д���������
                using (FileStream file = File.OpenRead(path))
                {
                    //һ��һ���ϴ�
                    byte[] bytes = new byte[2048];
                    //����ֵ ������˶����ֽ�
                    int contentlength = file.Read(bytes, 0, bytes.Length);

                    //ѭ���ϴ��ļ�����
                    while (contentlength != 0)
                    {
                        //д���ϴ���
                        upLoadStream.Write(bytes, 0, contentlength);
                        //д���ٶ�
                        contentlength = file.Read(bytes, 0, bytes.Length);
                    }

                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log("�ϴ��ɹ� " + fileName);
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "�ϴ�ʧ�� " + ex.Message);
            }
        });
    }
}
