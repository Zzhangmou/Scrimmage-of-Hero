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
    //资源服务器
    private string serverIP = "ftp://127.0.0.1";

    [MenuItem("Tools/AB/打开工具窗口")]
    private static void OpenWindow()
    {
        ABTools window = EditorWindow.GetWindowWithRect(typeof(ABTools), new Rect(0, 0, 350, 220)) as ABTools;
        window.Show();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 15), "平台选择");
        nowSelIndex = GUI.Toolbar(new Rect(10, 30, 250, 20), nowSelIndex, targetStrings);

        GUI.Label(new Rect(10, 60, 150, 15), "资源服务器地址");
        serverIP = GUI.TextField(new Rect(10, 80, 150, 20), serverIP);

        if (GUI.Button(new Rect(10, 110, 100, 40), "创建对比文件"))
        {
            CreateABCompareFile();
        }

        if (GUI.Button(new Rect(115, 110, 225, 40), "保存默认资源到StreamingAssets"))
        {
            MoveABToStreamAssets();
        }

        if (GUI.Button(new Rect(10, 160, 330, 40), "上传AB包和对比文件"))
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

        Debug.Log("AB包对比文件生成成功");
        AssetDatabase.Refresh();

    }
    private string GetMD5(string filePath)
    {
        //将文件以流形式打开
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //得到MD5码 16个字节 数组
            byte[] info = md5.ComputeHash(file);

            file.Close();

            //把16个字节转换为 16进制 拼接成字符串 减少MD5码长度
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
        //通过编辑器Selection类中的方法 获取再Project窗口中选中的资源 
        Object [] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //如果一个资源都没有选择 就没有必要处理后面的逻辑了
        if (selectedAsset.Length == 0)
            return;
        //用于拼接本地默认AB包资源信息的字符串
        string abCompareInfo = "";
        //遍历选中的资源对象
        foreach (Object asset in selectedAsset)
        {
            //通过Assetdatabase类 获取 资源的路径
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //截取路径当中的文件名 用于作为 StreamingAssets中的文件名
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //判断是否有.符号 如果有 证明有后缀 不处理
            if (fileName.IndexOf('.') != -1)
                continue;
            //你还可以在拷贝之前 去获取全路径 然后通过FIleInfo去获取后缀来判断 这样更加的准确

            //利用AssetDatabase中的API 将选中文件 复制到目标路径
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //获取拷贝到StreamingAssets文件夹中的文件的全部信息
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //拼接AB包信息到字符串中
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GenerateABCompare.GetMD5(fileInfo.FullName);
            //用一个符号隔开多个AB包信息
            abCompareInfo += "|";
        }
        //去掉最后一个|符号 为了之后拆分字符串方便
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //将本地默认资源的对比信息 存入文件
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //刷新窗口
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
                //上传
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
                //1.创建一个FTP连接用于上传
                FtpWebRequest req = FtpWebRequest.Create(new Uri(serverIP + "/AB/" + targetStrings[nowSelIndex] + "/" + fileName)) as FtpWebRequest;
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
