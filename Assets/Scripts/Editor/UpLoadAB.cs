using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class UpLoadAB
{
    //[MenuItem("Tools/AB/上传AB包和对比文件")]
    private static void UploadAllABFile()
    {
        string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
        DirectoryInfo directory = Directory.CreateDirectory(projectRootPath + "/AssetBundles/StandaloneWindows/");
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == "" || fileInfo.Extension == ".txt")
            {
                //上传
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
                //1.创建一个FTP连接用于上传
                FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/AB/" + fileName)) as FtpWebRequest;
                //2.设置一个通信凭证这样才能上传
                NetworkCredential networkCredential = new NetworkCredential("zhangmou", "maliang160");
                req.Credentials = networkCredential;
                //3.其它设置
                //  设置代理为nu11
                req.Proxy = null;
                //  请求完毕后是否关闭控制连接
                req.KeepAlive = false;
                //  操作命令-上传
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //  指定传输的类型 2进制
                req.UseBinary = true;
                //4.上传文件
                //  ftp的流对象
                Stream upLoadStream = req.GetRequestStream();
                //  读取文件信息 写入该流对象
                using (FileStream file = File.OpenRead(path))
                {
                    //一点一点上传
                    byte[] bytes = new byte[2048];
                    //返回值 代表读了多少字节
                    int contentlength = file.Read(bytes, 0, bytes.Length);

                    //循环上传文件数据
                    while (contentlength != 0)
                    {
                        //写入上传流
                        upLoadStream.Write(bytes, 0, contentlength);
                        //写完再读
                        contentlength = file.Read(bytes, 0, bytes.Length);
                    }

                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log("上传成功 " + fileName);
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "上传失败 " + ex.Message);
            }
        });
    }
}
