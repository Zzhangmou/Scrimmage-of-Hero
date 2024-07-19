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
    //�����AB���б�
    private Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();
    //����AB���б�
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    //�������б�
    private List<string> downloadList = new List<string>();


    public void CheckUpdate(UnityAction<bool> overCallBack, UnityAction<string> updateInfoCallBall)
    {
        //ÿ��ִ�����
        remoteABInfo.Clear();
        localABInfo.Clear();
        downloadList.Clear();

        //1����Զ����Դ�Ա��ļ�
        DownLoadABCompareFile((isOver) =>
        {
            updateInfoCallBall("��ʼ������Դ");
            if (isOver)
            {
                updateInfoCallBall("�Ա��ļ����ؽ���");
                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_Temp.txt");
                updateInfoCallBall("����Զ�˶Ա��ļ�");
                GetRemoteABCompareFileInfo(remoteInfo, remoteABInfo);
                updateInfoCallBall("����Զ�˶Ա��ļ����");
                //2���ر�����Դ�Ա��ļ�
                GetLocalABCompareFileInfo((isOver) =>
                {
                    if (isOver)
                    {
                        updateInfoCallBall("�������ضԱ��ļ����");
                        //3�Ա���Ϣ ����AB��
                        updateInfoCallBall("��ʼ�Ա�");
                        foreach (string abName in remoteABInfo.Keys)
                        {
                            //�µ�
                            if (!localABInfo.ContainsKey(abName))
                                downloadList.Add(abName);
                            else
                            {
                                //�ɵ��� ���ݲ�ͬ
                                if (remoteABInfo[abName].md5 != localABInfo[abName].md5)
                                    downloadList.Add(abName);

                                localABInfo.Remove(abName);
                            }
                        }
                        updateInfoCallBall("�Ա����");
                        //ɾ��û������
                        foreach (string abName in localABInfo.Keys)
                        {
                            if (File.Exists(Application.persistentDataPath + "/" + abName))
                                File.Delete(Application.persistentDataPath + "/" + abName);
                        }
                        //����AB��
                        updateInfoCallBall("���ظ���AB���ļ�");
                        DownLoadABFile((isOver) =>
                        {
                            if (isOver)
                            {
                                //���±���AB�Ա��ļ�
                                updateInfoCallBall("���±���AB�Ա��ļ�");
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
    /// ����AB���Ա��ļ�
    /// </summary>
    /// <param name="overCallBack"></param>
    public async void DownLoadABCompareFile(UnityAction<bool> overCallBack)
    {
        bool isOver = false;
        //�����������ش���
        int reDownLoadMaxNum = 5;
        //����Դ���������ضԱ��ļ�
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
        //�����ⲿ���
        overCallBack?.Invoke(isOver);
    }
    /// <summary>
    /// ��ȡԶ��AB���Ա���Ϣ
    /// </summary>
    public void GetRemoteABCompareFileInfo(string info, Dictionary<string, ABInfo> localABInfo)
    {
        //�������
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
    /// ����AB���Ա��ļ����� ������Ϣ
    /// </summary>
    public void GetLocalABCompareFileInfo(UnityAction<bool> overCallBack)
    {
        //����ɶ���д�ļ����д��ڶԱ��ļ� ˵��֮ǰ�Ѿ����ظ��¹���
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
        //��û��֤��û��Ĭ����Դ
        else
            overCallBack(true);
    }

    private IEnumerator GetLocalABCompareFileInfo(string FilePath, UnityAction<bool> overCallBack)
    {
        UnityWebRequest req = UnityWebRequest.Get(FilePath);
        yield return req.SendWebRequest();
        //��ȡ�ļ��ɹ�
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
        //���سɹ���ʱlist
        List<string> tempList = new List<string>();
        //�����������ش���
        int reDownLoadMaxNum = 5;
        //��ǰ���ظ���
        int downLoadOverNum = 0;
        //������ظ���
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

            //1.����һ��FTP������������
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/AB/" + pInfo + '/' + fileName)) as FtpWebRequest;
            //2.����һ��ͨ��ƾ֤�����������أ������˺ſ��Բ����ã�
            NetworkCredential networkCredential = new NetworkCredential("zhangmou", "maliang160");
            req.Credentials = networkCredential;
            //3.��������
            //  ���ô���Ϊnull
            req.Proxy = null;
            //  ������Ϻ��Ƿ�رտ�������
            req.KeepAlive = false;
            //  ��������-����
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //  ָ����������� 2����
            req.UseBinary = true;
            //4.�����ļ�
            //  ftp��������
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downLoadStream = res.GetResponseStream();
            //  ��ȡ�ļ���Ϣ д���������
            using (FileStream file = File.Create(localPath))
            {
                //һ��һ������
                byte[] bytes = new byte[2048];
                //����ֵ ������˶����ֽ�
                int contentlength = downLoadStream.Read(bytes, 0, bytes.Length);

                //ѭ�������ļ�����
                while (contentlength != 0)
                {
                    //д�뱾���ļ���
                    file.Write(bytes, 0, contentlength);
                    //д���ٶ�
                    contentlength = downLoadStream.Read(bytes, 0, bytes.Length);
                }

                file.Close();
                downLoadStream.Close();
                Debug.Log("���سɹ� " + fileName);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(fileName + "����ʧ�� " + ex.Message);
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
