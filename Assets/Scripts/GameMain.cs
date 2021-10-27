using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using NetWorkFK;

namespace Common
{
    /// <summary>
    /// 游戏主要入口
    /// </summary>
    public class GameMain : MonoSingleton<GameMain>
    {
        public string id = "";

        public override void Init()
        {
            base.Init();
            LuaManager.Instance.DoLuaFile("Main");
            //NetManager.Connect("127.0.0.1", 18188);
            NetManager.Connect("server.natappfree.cc", 43969);

        }

        private void Update()
        {
            NetManager.Update();
        }
    }
}