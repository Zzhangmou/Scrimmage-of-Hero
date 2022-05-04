using NetWorkFK;
using ProtoBuf;
using proto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class GameMainHelper
    {
        public static void OnShow()
        {
            NetManager.AddMsgListener("MsgGetUserInfo", OnMsgGetUserInfo);
        }
        public static void OnClose()
        {
            NetManager.RemoveMsgListener("MsgGetUserInfo", OnMsgGetUserInfo);
        }

        private static void OnMsgGetUserInfo(IExtensible msgBase)
        {
            MsgGetUserInfo msg = (MsgGetUserInfo)msgBase;
            int score = msg.win * 100 - msg.lose * 10;
            CallLuaHelper.SetUserInfo(msg.userName, score.ToString());
        }
    }
}