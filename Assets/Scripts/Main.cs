using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System;

namespace ns
{
    /// <summary>
    /// 
    /// </summary>
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            LuaManager.Instance.DoLuaFile("Main");
            NetManager.AddListener("Enter", OnEnter);
            NetManager.AddListener("Move", OnMove);
            NetManager.AddListener("Leave", OnLeave);
            //NetManager.Connect("127.0.0.1", 8080);
        }

        private void OnLeave(string str)
        {
            Debug.Log("OnLeave " + str);
        }

        private void OnMove(string str)
        {
            Debug.Log("OnMove " + str);
        }

        private void OnEnter(string str)
        {
            Debug.Log("OnEnter " + str);
        }
    }
}