using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.IO;
using System.Net;
using System;
using DG.Tweening.Plugins.Core.PathCore;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AbUpdateManager : MonoSingleton<AbUpdateManager>
{
    //服务端AB包列表
    private Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();
    //本地AB包列表
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    //待下载列表
    private List<string> downloadList = new List<string>();


    public void CheckUpdate(UnityAction<bool> overCallBack, UnityAction<string> updateInfoCallBall)
    {
        //每次执行清空
        remoteABInfo.Clear();
        localABInfo.Clear();
        downloadList.Clear();

        //1加载远端资源对比文件
        DownLoadABCompareFile((isOver) =>
        {
            updateInfoCallBall("开始更新资源");
            if (isOver)
            {
                updateInfoCallBall("对比文件下载结束");
                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_Temp.txt");
                updateInfoCallBall("解析远端对比文件");
                GetRemoteABCompareFileInfo(remoteInfo, remoteABInfo);
                updateInfoCallBall("解析远端对比文件完成");
                //2加载本地资源对比文件
                GetLocalABCompareFileInfo((isOver) =>
                {
                    if (isOver)
                    {
                        updateInfoCallBall("解析本地对比文件完成");
                        //3对比信息 下载AB包
                        updateInfoCallBall("开始对比");
                        foreach (string abName in remoteABInfo.Keys)
                        {
                            //新的
                            if (!localABInfo.ContainsKey(abName))
                                downloadList.Add(abName);
                            else
                            {
                                //旧的有 内容不同
                                if (remoteABInfo[abName].md5 != localABInfo[abName].md5)
                                    downloadList.Add(abName);

                                localABInfo.Remove(abName);
                            }
                        }
                        updateInfoCallBall("对比完成");
                        //删除没用内容
                        foreach (string abName in localABInfo.Keys)
                        {
                            if (File.Exists(Application.persistentDataPath + "/" + abName))
                                File.Delete(Application.persistentDataPath + "/" + abName);
                        }
                        //下载AB包
                        updateInfoCallBall("下载更新AB包文件");
                        DownLoadABFile((isOver) =>
                        {
                            if (isOver)
                            {
                                //更新本地AB对比文件
                                updateInfoCallBall("更新本地AB对比文件");
                                File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", remoteInfo);
                            }
                            overCallBack(isOver);
                        }, updateInfoCallBall);
                    }
                    else
                    {
                        overCallBack(false);
                    }
                });
            }
            else
            {
                overCallBack(false);
            }
        });
    }

    /// <summary>
    /// 下载AB包对比文件
    /// </summary>
    /// <param name="overCallBack"></param>
    public async void DownLoadABCompareFile(UnityAction<bool> overCallBack)
    {
        bool isOver = false;
        //尝试重新下载次数
        int reDownLoadMaxNum = 5;
        //从资源服务器下载对比文件
        print(Application.persistentDataPath);
        string localPath = Application.persistentDataPath;
        while (!isOver && reDownLoadMaxNum > 0)
        {
            await Task.Run(() =>
            {
                isOver = DownLoadFile("ABCompareInfo.txt", localPath + "/ABCompareInfo_Temp.txt");
            });
            reDownLoadMaxNum--;
        }
        //告诉外部结果
        overCallBack?.Invoke(isOver);
    }
    /// <summary>
    /// 获取远端AB包对比信息
    /// </summary>
    public void GetRemoteABCompareFileInfo(string info, Dictionary<string, ABInfo> localABInfo)
    {
        //拆分内容
        //string info = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_Temp.txt");
        string[] strs = info.Split('|');
        string[] infos;
        for (int i = 0; i < strs.Length; i++)
        {
            infos = strs[i].Split(' ');
            localABInfo.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));
        }
    }
    /// <summary>
    /// 本地AB包对比文件加载 解析信息
    /// </summary>
    public void GetLocalABCompareFileInfo(UnityAction<bool> overCallBack)
    {
        //如果可读可写文件夹中存在对比文件 说明之前已经下载更新过了
        if (File.Exists(Application.persistentDataPath + "/ABCompareInfo.txt"))
        {
            StartCoroutine(GetLocalABCompareFileInfo("File:///" + Application.persistentDataPath + "/ABCompareInfo.txt", overCallBack));
        }
        else if (File.Exists(Application.streamingAssetsPath + "/ABCompareInfo.txt"))
        {
            string path =
#if UNITY_ANDRIOD
Application.streamingAssetsPath;
#else
"File:///" + Application.streamingAssetsPath;
#endif
            StartCoroutine(GetLocalABCompareFileInfo(path + "/ABCompareInfo.txt", overCallBack));
        }
        //都没有证明没有默认资源
        else
            overCallBack(true);
    }

    private IEnumerator GetLocalABCompareFileInfo(string FilePath, UnityAction<bool> overCallBack)
    {
        UnityWebRequest req = UnityWebRequest.Get(FilePath);
        yield return req.SendWebRequest();
        //获取文件成功
        if (req.result == UnityWebRequest.Result.Success)
        {
            GetRemoteABCompareFileInfo(req.downloadHandler.text, localABInfo);
            overCallBack(true);
        }
        else
        {
            overCallBack(false);
        }
    }

    public async void DownLoadABFile(UnityAction<bool> overCallBack, UnityAction<string> updatePro)
    {
        //foreach (string name in remoteABInfo.Keys)
        //{
        //    downloadList.Add(name);
        //}
        bool isOver = false;
        //下载成功临时list
        List<string> tempList = new List<string>();
        //尝试重新下载次数
        int reDownLoadMaxNum = 5;
        //当前下载个数
        int downLoadOverNum = 0;
        //最大下载个数
        int downLoadMaxNum = downloadList.Count;
        string localPath = Application.persistentDataPath + '/';
        while (downloadList.Count > 0 && reDownLoadMaxNum > 0)
        {
            for (int i = 0; i < downloadList.Count; i++)
            {
                isOver = false;
                await Task.Run(() =>
                {
                    isOver = DownLoadFile(downloadList[i], localPath + downloadList[i]);
                });
                if (isOver)
                {
                    updatePro(++downLoadOverNum + "/" + downLoadMaxNum);
                    tempList.Add(downloadList[i]);
                }
            }
            for (int i = 0; i < tempList.Count; i++)
                downloadList.Remove(tempList[i]);
            --reDownLoadMaxNum;
        }
        if (downloadList.Count == 0)
            overCallBack(downloadList.Count == 0);
    }

    private bool DownLoadFile(string fileName, string localPath)
    {
        try
        {
            string pInfo =
#if UNITY_IOS
    "IOS";
#elif UNITY_ANDROID
    "Android";
#else
    "PC";
#endif

            //1.创建一个FTP连接用于下载
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/AB/" + pInfo + '/' + fileName)) as FtpWebRequest;
            //2.设置一个通信凭证这样才能下载（匿名账号可以不设置）
            NetworkCredential networkCredential = new NetworkCredential("zhangmou", "maliang160");
            req.Credentials = networkCredential;
            //3.其它设置
            //  设置代理为null
            req.Proxy = null;
            //  请求完毕后是否关闭控制连接
            req.KeepAlive = false;
            //  操作命令-下载
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //  指定传输的类型 2进制
            req.UseBinary = true;
            //4.下载文件
            //  ftp的流对象
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downLoadStream = res.GetResponseStream();
            //  读取文件信息 写入该流对象
            using (FileStream file = File.Create(localPath))
            {
                //一点一点下载
                byte[] bytes = new byte[2048];
                //返回值 代表读了多少字节
                int contentlength = downLoadStream.Read(bytes, 0, bytes.Length);

                //循环下载文件数据
                while (contentlength != 0)
                {
                    //写入本地文件流
                    file.Write(bytes, 0, contentlength);
                    //写完再读
                    contentlength = downLoadStream.Read(bytes, 0, bytes.Length);
                }

                file.Close();
                downLoadStream.Close();
                Debug.Log("下载成功 " + fileName);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(fileName + "下载失败 " + ex.Message);
            return false;
        }
    }

    public class ABInfo
    {
        public string name;
        public long size;
        public string md5;

        public ABInfo(string name, string size, string md5)
        {
            this.name = name;
            this.size = long.Parse(size);
            this.md5 = md5;
        }
    }
}
