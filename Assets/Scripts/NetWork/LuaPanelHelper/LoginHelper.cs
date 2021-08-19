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
    /// 登陆
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
            Debug.Log("连接失败");
        }

        private static void OnConnectSucc(string str)
        {
            Debug.Log("连接成功");
        }

        private static void OnMsgLogin(IExtensible msgBase)
        {
            MsgLogin msg = (MsgLogin)msgBase;
            if (msg.result == 0)
            {
                Debug.Log("登陆成功");
                GameMain.Instance.id = msg.id;//设置id
                CallLuaHelper.PanelClose("LoginPanel");//关闭面板
                CallLuaHelper.PanelClose("StartShowPanel");
                CallLuaHelper.PanelShow("GameMainPanel");//打开主面板
            }
            else
            {
                Debug.Log("登陆失败  返回码为: " + msg.result);
                switch (msg.result)
                {
                    case -1:
                        CallLuaHelper.PanelShowTip("账号或密码错误");
                        break;
                    case -2:
                        CallLuaHelper.PanelShowTip("用户已经登陆");
                        break;
                    case -3:
                        CallLuaHelper.PanelShowTip("获取玩家数据出错");
                        break;
                }
            }
        }
    }
}

