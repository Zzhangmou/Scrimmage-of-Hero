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
        public string id = "";
        public override void Init()
        {
            base.Init();
            LuaManager.Instance.DoLuaFile("Main");
            NetManager.Connect("1.116.80.9", 18188);
        }

        private void Update()
        {
            NetManager.Update();
        }
    }
}