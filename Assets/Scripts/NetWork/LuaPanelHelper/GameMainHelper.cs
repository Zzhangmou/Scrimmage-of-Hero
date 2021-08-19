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
            CallLuaHelper.SetUserInfo(msg.userName, msg.win + "Ê¤" + msg.lose + "¸º");
        }
    }
}