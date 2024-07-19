using Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Common
{
    /// <summary>
    /// lua������ �ṩlua������
    /// </summary>
    public class LuaManager : MonoSingleton<LuaManager>
    {
        private LuaEnv luaEnv;

        /// <summary>
        /// �õ�Lua�е�_G
        /// </summary>
        public LuaTable Global { get { return luaEnv.Global; } }

        public override void Init()
        {
            base.Init();
            if (luaEnv != null) return;
            luaEnv = new LuaEnv();

            //����lua�ű� �ض���
            luaEnv.AddLoader(MyCustomLoader);
            luaEnv.AddLoader(MyCustomABLoader);
        }
        //�Զ�ִ��
        private byte[] MyCustomLoader(ref string filePath)
        {
            //���Դ���Ĳ�����ʲô
            Debug.Log(filePath);
            
            string path = Application.dataPath + "/ArtRes//Lua/" + filePath + ".lua";

            //�ж��ļ��Ƿ����
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.Log("�ض���ʧ��,�ļ���Ϊ:" + filePath);
            }
            return null;
        }
        /// <summary>
        /// �ض������AB���е�Lua�ű�
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private byte[] MyCustomABLoader(ref string filepath)
        {
            #region
            //Debug.Log("����AB������ �ض�����");
            ////��AB���м���lua�ļ�
            ////����AB��
            //string path = Application.streamingAssetsPath + "/lua";
            //AssetBundle ab = AssetBundle.LoadFromFile(path);
            ////����lua�ļ� ����
            //TextAsset tx = ab.LoadAsset<TextAsset>(filepath + ".lua");
            ////����lua�ļ� byte����
            //return tx.bytes;
            #endregion
            //ͨ��Ab�������� ����lua�ű���Դ
            TextAsset lua = AbManager.Instance.LoadRes<TextAsset>("lua", filepath + ".lua");
            if (lua != null)
                return lua.bytes;
            else
                Debug.Log("MyCustomABLoader�ض���ʧ��,�ļ���Ϊ:" + filepath);

            return null;
        }
        /// <summary>
        /// ����lua�ļ��� ִ��lua�ű�
        /// </summary>
        /// <param name="fileName"></param>
        public void DoLuaFile(string fileName)
        {
            string str = string.Format("require('{0}')", fileName);
            DoString(str);
        }
        /// <summary>
        /// ִ��lua����
        /// </summary>
        /// <param name="str"></param>
        public void DoString(string str)
        {
            if (luaEnv == null)
            {
                Debug.Log("������δ��ʼ��");
            }
            luaEnv.DoString(str);
        }
        /// <summary>
        /// �ͷ� lua����
        /// </summary>
        public void Tick()
        {
            if (luaEnv == null)
            {
                Debug.Log("������δ��ʼ��");
            }
            luaEnv.Tick();
        }
        /// <summary>
        /// ���ٽ�����
        /// </summary>
        public void Dispose()
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
