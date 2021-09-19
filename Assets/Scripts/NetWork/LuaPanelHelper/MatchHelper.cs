using NetWorkFK;
using proto;
using ProtoBuf;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class MatchHelper
    {
        public static void OnShow()
        {
            NetManager.AddMsgListener("MsgEnterMatch", OnMsgEnterMatch);
            NetManager.AddMsgListener("MsgLeaveMatch", OnMsgLeaveMatch);
        }

        public static void OnClose()
        {
            NetManager.RemoveMsgListener("MsgEnterMatch", OnMsgEnterMatch);
            NetManager.RemoveMsgListener("MsgLeaveMatch", OnMsgLeaveMatch);
        }

        private static void OnMsgLeaveMatch(IExtensible msgBase)
        {
            MsgLeaveMatch msg = (MsgLeaveMatch)msgBase;
            UpdateMatch(msg.currentMatchNum);
        }

        private static void OnMsgEnterMatch(IExtensible msgBase)
        {
            MsgEnterMatch msg = (MsgEnterMatch)msgBase;
            UpdateMatch(msg.currentMatchNum);
        }
        private static void UpdateMatch(int num)
        {
            string text = num + "/6";
            CallLuaHelper.UpdateText(text);
        }
    }
}