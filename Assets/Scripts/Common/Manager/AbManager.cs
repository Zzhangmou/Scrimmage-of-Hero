using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// AB��������
    /// </summary>
    public class AbManager : MonoSingleton<AbManager>
    {
        //����
        private AssetBundle mainAB = null;
        //��������ȡ�õ������ļ�
        private AssetBundleManifest manifest = null;
        //�洢���ع���AB��
        private Dictionary<string, AssetBundle> abDic;
        /// <summary>
        /// AB�����·��
        /// </summary>
        private string PathUrl { get { return Application.streamingAssetsPath + "/"; } }
        /// <summary>
        /// ������
        /// </summary>
        private string MainAbName
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return "PC";
#elif UNITY_IOS
        reuten "IOS";
#elif UNITY_ANDROID
                return "Android";
#endif
            }
        }
        public override void Init()
        {
            base.Init();
            abDic = new Dictionary<string, AssetBundle>();
        }

        /// <summary>
        /// ����AB��
        /// </summary>
        /// <param name="abName">������</param>
        public void LoadAB(string abName)
        {
            //����AB��
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(PathUrl + MainAbName);
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //��ȡ�����������Ϣ
            AssetBundle ab = null;
            string[] strs = manifest.GetAllDependencies(abName);
            for (int i = 0; i < strs.Length; i++)
            {
                //�ж��Ƿ����
                if (!abDic.ContainsKey(strs[i]))
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
            }
            //������Դ��Դ��
            //���û�м��ع� �ټ���
            if (!abDic.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
        }
        /// <summary>
        /// ͬ������
        /// </summary>
        /// <param name="abName">������</param>
        /// <param name="resName">��Դ��</param>
        /// <returns></returns>
        public Object LoadRes(string abName, string resName)
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            Object obj = abDic[abName].LoadAsset(resName);
            if (obj is GameObject)
                return Instantiate(obj);
            else
                return obj;
        }

        /// <summary>
        /// ͬ������ ����typeָ������
        /// </summary>
        /// <param name="abName">������</param>
        /// <param name="resName">��Դ��</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public Object LoadRes(string abName, string resName, System.Type type)
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            Object obj = abDic[abName].LoadAsset(resName, type);
            if (obj is GameObject)
                return Instantiate(obj);
            else
                return obj;
        }

        /// <summary>
        /// ͬ������ ���ݷ���ָ������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="abName">������</param>
        /// <param name="resName">��Դ��</param>
        /// <returns></returns>
        public T LoadRes<T>(string abName, string resName) where T : Object
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            T obj = abDic[abName].LoadAsset<T>(resName);
            if (obj is GameObject)
                return Instantiate(obj);
            else
                return obj;
        }
        //�첽����
        //������첽���� AB����û��ʹ���첽����
        //��AB���� ������Դʱ ʹ���첽
        /// <summary>
        /// ���������첽������Դ
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="callBack"></param>
        public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
        {
            StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
        }

        private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
            yield return abr;
            //�첽���ؽ����� ͨ��ί�д��ݸ��ⲿʹ��
            if (abr.asset is GameObject)
                callBack(Instantiate(abr.asset));
            else
                callBack(abr.asset);
        }
        /// <summary>
        /// ����Type�첽������Դ
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <param name="callBack"></param>
        public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
        {
            StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
        }

        private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
            yield return abr;
            //�첽���ؽ����� ͨ��ί�д��ݸ��ⲿʹ��
            if (abr.asset is GameObject)
                callBack(Instantiate(abr.asset));
            else
                callBack(abr.asset);
        }
        /// <summary>
        /// ���ݷ����첽������Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="callBack"></param>
        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
        {
            StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
        }
        private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
        {
            //����AB��
            LoadAB(abName);

            //����Ŀ���
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abr;
            //�첽���ؽ����� ͨ��ί�д��ݸ��ⲿʹ��
            if (abr.asset is GameObject)
                callBack(Instantiate(abr.asset) as T);
            else
                callBack(abr.asset as T);
        }
        /// <summary>
        /// ������ж��
        /// </summary>
        /// <param name="abName"></param>
        public void Unload(string abName)
        {
            if (abDic.ContainsKey(abName))
            {
                abDic[abName].Unload(false);
                abDic.Remove(abName);
            }
        }

        /// <summary>
        /// ж�����а�
        /// </summary>
        public void ClearAB()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abDic.Clear();
            mainAB = null;
            manifest = null;
        }
    }
}
