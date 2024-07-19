using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class UpLoadAB
{
    //[MenuItem("Tools/AB/�ϴ�AB���ͶԱ��ļ�")]
    private static void UploadAllABFile()
    {
        string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
        DirectoryInfo directory = Directory.CreateDirectory(projectRootPath + "/AssetBundles/StandaloneWindows/");
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

    private async static void FTPUpLoadFile(string path, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                //1.����һ��FTP���������ϴ�
                FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/AB/" + fileName)) as FtpWebRequest;
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
