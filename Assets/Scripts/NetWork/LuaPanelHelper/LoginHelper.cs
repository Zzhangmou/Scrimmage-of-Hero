using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkFK;
using proto;
using ns;

namespace Helper
{
    /// <summary>
    /// ��½
    /// </summary>
    public static class LoginHelper
    {
        public static void OnShow()
        {
            NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
            NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
            NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        }

        public static void OnClose()
        {
            NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
            NetManager.RemoveEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
            NetManager.RemoveEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        }

        private static void OnConnectFail(string str)
        {
            Debug.Log("����ʧ��");
        }

        private static void OnConnectSucc(string str)
        {
            Debug.Log("���ӳɹ�");
        }

        private static void OnMsgLogin(IExtensible msgBase)
        {
            MsgLogin msg = (MsgLogin)msgBase;
            if (msg.result == 0)
            {
                Debug.Log("��½�ɹ�");
                GameMain.Instance.id = msg.id;//����id
                CallLuaHelper.PanelClose("LoginPanel");//�ر����
                CallLuaHelper.PanelClose("StartShowPanel");
                CallLuaHelper.PanelShow("GameMainPanel");//�������
            }
            else
            {
                Debug.Log("��½ʧ��  ������Ϊ: " + msg.result);
                switch (msg.result)
                {
                    case -1:
                        CallLuaHelper.PanelShowTip("�˺Ż��������");
                        break;
                    case -2:
                        CallLuaHelper.PanelShowTip("�û��Ѿ���½");
                        break;
                    case -3:
                        CallLuaHelper.PanelShowTip("��ȡ������ݳ���");
                        break;
                }
            }
        }
    }
}

