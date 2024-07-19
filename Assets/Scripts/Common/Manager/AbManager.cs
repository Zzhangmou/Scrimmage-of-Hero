using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// AB包管理器
    /// </summary>
    public class AbManager : MonoSingleton<AbManager>
    {
        //主包
        private AssetBundle mainAB = null;
        //依赖包获取用的配置文件
        private AssetBundleManifest manifest = null;
        //存储加载过的AB包
        private Dictionary<string, AssetBundle> abDic;
        /// <summary>
        /// AB包存放路径
        /// </summary>
        //private string PathUrl { get { return Application.streamingAssetsPath + "/"; } }
        /// <summary>
        /// 主包名
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

        //如果可读可写返回新资源 没有说明是默认资源 
        public AssetBundle GetRealAB(string abName)
        {
            if (File.Exists(Application.persistentDataPath + '/' + abName))
            {
                return AssetBundle.LoadFromFile(Application.persistentDataPath + '/' + abName);
            }
            return AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abName);
        }
        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName">主包名</param>
        public void LoadAB(string abName)
        {
            //加载AB包
            if (mainAB == null)
            {
                //mainAB = AssetBundle.LoadFromFile(PathUrl + MainAbName);
                mainAB = GetRealAB(MainAbName);
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //获取依赖包相关信息
            AssetBundle ab;
            string[] strs = manifest.GetAllDependencies(abName);
            for (int i = 0; i < strs.Length; i++)
            {
                //判断是否加载
                if (!abDic.ContainsKey(strs[i]))
                {
                    ab = GetRealAB(strs[i]);
                    abDic.Add(strs[i], ab);
                }
            }
            //加载资源来源包
            //如果没有加载过 再加载
            if (!abDic.ContainsKey(abName))
            {
                ab = GetRealAB(abName);
                abDic.Add(abName, ab);
            }
        }
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="abName">主包名</param>
        /// <param name="resName">资源名</param>
        /// <returns></returns>
        public Object LoadRes(string abName, string resName)
        {
            //加载AB包
            LoadAB(abName);

            //加载目标包
            return abDic[abName].LoadAsset(resName);
        }

        /// <summary>
        /// 同步加载 根据type指定类型
        /// </summary>
        /// <param name="abName">主包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Object LoadRes(string abName, string resName, System.Type type)
        {
            //加载AB包
            LoadAB(abName);

            //加载目标包
            return abDic[abName].LoadAsset(resName, type);
        }

        /// <summary>
        /// 同步加载 根据泛型指定类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="abName">主包名</param>
        /// <param name="resName">资源名</param>
        /// <returns></returns>
        public T LoadRes<T>(string abName, string resName) where T : Object
        {
            //加载AB包
            LoadAB(abName);

            //加载目标包
            return abDic[abName].LoadAsset<T>(resName);
        }
        //异步加载
        //这里的异步加载 AB包并没有使用异步加载
        //从AB包中 加载资源时 使用异步
        /// <summary>
        /// 根据名字异步加载资源
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
            //加载AB包
            LoadAB(abName);

            //加载目标包
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
            yield return abr;
            //异步加载结束后 通过委托传递给外部使用
            callBack(abr.asset);
        }
        /// <summary>
        /// 根据Type异步加载资源
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
            //加载AB包
            LoadAB(abName);

            //加载目标包
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
            yield return abr;
            //异步加载结束后 通过委托传递给外部使用
            callBack(abr.asset);
        }
        /// <summary>
        /// 根据泛型异步加载资源
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
            //加载AB包
            LoadAB(abName);

            //加载目标包
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abr;
            //异步加载结束后 通过委托传递给外部使用
            callBack(abr.asset as T);
        }
        /// <summary>
        /// 单个包卸载
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
        /// 卸载所有包
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
