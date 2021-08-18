using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkFK;
using proto;
using ProtoBuf;
using System;

namespace Helper
{
    /// <summary>
    /// ×¢²á
    /// </summary>
    public static class RegisterHelper
    {
        public static void OnShow()
        {
            NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        }
        public static void OnClose()
        {
            NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
        }
        private static void OnMsgRegister(IExtensible msgBase)
        {
            MsgRegister msg = (MsgRegister)msgBase;
            if (msg.result == 0)
            {
                Debug.Log("×¢²á³É¹¦");
                CallLuaHelper.PanelClose("RegisterPanel");
                CallLuaHelper.PanelShowTip("×¢²á³É¹¦");
            }
            else
            {
                Debug.Log("×¢²áÊ§°Ü");
                CallLuaHelper.PanelShowTip("×¢²áÊ§°Ü,×¢²áÕËºÅÒÑ´æÔÚ");
            }
        }
    }
}

