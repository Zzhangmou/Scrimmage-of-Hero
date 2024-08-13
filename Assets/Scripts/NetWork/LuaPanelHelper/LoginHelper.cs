using ProtoBuf;
using UnityEngine;
using NetWorkFK;
using proto;
using Common;

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
            Debug.Log("连接失败" + str);
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
                CallLuaHelper.PanelClose("LoginView");//关闭面板
                CallLuaHelper.PanelClose("StartShowView");
                CallLuaHelper.PanelShow("ShowMainView");//打开主面板
            }
            else
            {
                Debug.Log("登陆失败  返回码为: " + msg.result);
                switch (msg.result)
                {
                    case -1:
                        CallLuaHelper.ShowMessage("账号或密码错误");
                        PlayerPrefs.DeleteKey("ID");
                        PlayerPrefs.DeleteKey("PW");
                        break;
                    case -2:
                        CallLuaHelper.ShowMessage("用户已经登陆");
                        break;
                    case -3:
                        CallLuaHelper.ShowMessage("获取玩家数据出错");
                        break;
                }
            }
        }
    }
}

