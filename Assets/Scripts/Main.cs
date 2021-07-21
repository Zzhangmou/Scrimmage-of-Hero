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
    /// 
    /// </summary>
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            //LuaManager.Instance.DoLuaFile("Main");
            NetManager.Connect("127.0.0.1", 8080);
        }

        private void Update()
        {
            NetManager.Update();
        }
    }
}