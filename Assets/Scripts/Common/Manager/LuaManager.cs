using Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Common
{
    /// <summary>
    /// lua管理器 提供lua解析器
    /// </summary>
    public class LuaManager : MonoSingleton<LuaManager>
    {
        private LuaEnv luaEnv;

        /// <summary>
        /// 得到Lua中的_G
        /// </summary>
        public LuaTable Global { get { return luaEnv.Global; } }

        public override void Init()
        {
            base.Init();
            if (luaEnv != null) return;
            luaEnv = new LuaEnv();

            //加载lua脚本 重定向
            luaEnv.AddLoader(MyCustomLoader);
            luaEnv.AddLoader(MyCustomABLoader);
        }
        //自动执行
        private byte[] MyCustomLoader(ref string filePath)
        {
            //测试传入的参数是什么
            Debug.Log(filePath);
            
            string path = Application.dataPath + "/ArtRes//Lua/" + filePath + ".lua";

            //判断文件是否存在
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.Log("重定向失败,文件名为:" + filePath);
            }
            return null;
        }
        /// <summary>
        /// 重定向加载AB包中的Lua脚本
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private byte[] MyCustomABLoader(ref string filepath)
        {
            #region
            //Debug.Log("进入AB包加载 重定向函数");
            ////从AB包中加载lua文件
            ////加载AB包
            //string path = Application.streamingAssetsPath + "/lua";
            //AssetBundle ab = AssetBundle.LoadFromFile(path);
            ////加载lua文件 返回
            //TextAsset tx = ab.LoadAsset<TextAsset>(filepath + ".lua");
            ////加载lua文件 byte数组
            //return tx.bytes;
            #endregion
            //通过Ab包管理器 加载lua脚本资源
            TextAsset lua = AbManager.Instance.LoadRes<TextAsset>("lua", filepath + ".lua");
            if (lua != null)
                return lua.bytes;
            else
                Debug.Log("MyCustomABLoader重定向失败,文件名为:" + filepath);

            return null;
        }
        /// <summary>
        /// 传入lua文件名 执行lua脚本
        /// </summary>
        /// <param name="fileName"></param>
        public void DoLuaFile(string fileName)
        {
            string str = string.Format("require('{0}')", fileName);
            DoString(str);
        }
        /// <summary>
        /// 执行lua语言
        /// </summary>
        /// <param name="str"></param>
        public void DoString(string str)
        {
            if (luaEnv == null)
            {
                Debug.Log("解析器未初始化");
            }
            luaEnv.DoString(str);
        }
        /// <summary>
        /// 释放 lua垃圾
        /// </summary>
        public void Tick()
        {
            if (luaEnv == null)
            {
                Debug.Log("解析器未初始化");
            }
            luaEnv.Tick();
        }
        /// <summary>
        /// 销毁解释器
        /// </summary>
        public void Dispose()
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
