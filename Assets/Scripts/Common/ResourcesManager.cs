using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourcesManager
    {
        private static Dictionary<string, string> configMap;
        //作用:初始化类的静态数据成员
        //时机:类被加载时执行一次
        static ResourcesManager()
        {
            //加载文件
            string fileContent = GetConfigFile("ConfigMap.txt");
            //解析文件  -->Dic
            BuildMap(fileContent);
        }

        public static string GetConfigFile(string fileName)
        {
            string url;
            //   string url = "file://" + Application.streamingAssetsPath + "/ConfigMap.txt";
            #region url平台配置streamingAssetsPath 
#if UNITY_EDITOR||UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
#elif UNITY_IPHONE
            url = "file://" + Application.dataPath + "/Raw/" + fileName;
#elif UNITY_ANDROID
            url = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
#endif
            #endregion
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.SendWebRequest();
            while (true)
            {
                if (unityWebRequest.downloadHandler.isDone)
                    return unityWebRequest.downloadHandler.text;
            }
        }

        public static void BuildMap(string fileContent)
        {
            configMap = new Dictionary<string, string>();
            //字符串读取器 提供逐行读取字符串功能
            using StringReader reader = new StringReader(fileContent);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //解析
                string[] keyValue = line.Split('=');
                configMap.Add(keyValue[0], keyValue[1]);
            }
        }
        public static T Load<T>(string prefabName) where T : Object
        {
            string prefabPath = configMap[prefabName];
            return Resources.Load<T>(prefabPath);
        }
    }
}

