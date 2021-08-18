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
    /// ע��
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
                Debug.Log("ע��ɹ�");
                CallLuaHelper.PanelClose("RegisterPanel");
                CallLuaHelper.PanelShowTip("ע��ɹ�");
            }
            else
            {
                Debug.Log("ע��ʧ��");
                CallLuaHelper.PanelShowTip("ע��ʧ��,ע���˺��Ѵ���");
            }
        }
    }
}

