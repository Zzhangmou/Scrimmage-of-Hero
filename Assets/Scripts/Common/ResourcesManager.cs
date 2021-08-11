using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// ��Դ������
    /// </summary>
    public class ResourcesManager
    {
        private static Dictionary<string, string> configMap;
        //����:��ʼ����ľ�̬���ݳ�Ա
        //ʱ��:�౻����ʱִ��һ��
        static ResourcesManager()
        {
            //�����ļ�
            string fileContent = GetConfigFile("ConfigMap.txt");
            //�����ļ�  -->Dic
            BuildMap(fileContent);
        }

        public static string GetConfigFile(string fileName)
        {
            string url;
            //   string url = "file://" + Application.streamingAssetsPath + "/ConfigMap.txt";
            #region urlƽ̨����streamingAssetsPath 
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
            //�ַ�����ȡ�� �ṩ���ж�ȡ�ַ�������
            using StringReader reader = new StringReader(fileContent);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //����
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

