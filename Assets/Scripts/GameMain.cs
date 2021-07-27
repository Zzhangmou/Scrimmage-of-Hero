using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System;
using NetWorkFK;

namespace ns
{
    /// <summary>
    /// ��Ϸ��Ҫ���
    /// </summary>
    public class GameMain : MonoSingleton<GameMain>
    {
        public override void Init()
        {
            base.Init();
            LuaManager.Instance.DoLuaFile("Main");
            NetManager.Connect("127.0.0.1", 8080);
        }

        private void Update()
        {
            NetManager.Update();
        }
    }
}