using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public static class NetManager
    {
        private static Socket socket;
        //接收缓冲区
        private static byte[] readBuff = new byte[512];
        /// <summary>
        /// 委托类型
        /// </summary>
        /// <param name="str"></param>
        public delegate void MsgListener(string str);
        //监听列表
        private static Dictionary<string, MsgListener> listeners = new Dictionary<string, MsgListener>();
        //消息列表
        private static List<string> msgList = new List<string>();
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="msgName">监听名称</param>
        /// <param name="listener">监听事件</param>
        public static void AddListener(string msgName, MsgListener listener)
        {
            listeners[msgName] = listener;
        }
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <returns></returns>
        public static string GetDesc()
        {
            if (socket == null) return "";
            if (!socket.Connected) return "";
            return socket.LocalEndPoint.ToString();
        }
        public static void Connect(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }

        public static void Send(string sendStr)
        {
            if (socket == null) return;
            if (!socket.Connected) return;

            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
        }

        public static void Update()
        {
            if (msgList.Count <= 0) return;
            string msgStr = msgList[0];
            msgList.RemoveAt(0);
            string[] split = msgStr.Split('|');
            string msgName = split[0];//协议名
            string msgArgs = split[1];//协议体

            //监听回调
            if (listeners.ContainsKey(msgName))
                listeners[msgName](msgArgs);
        }
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                socket.BeginReceive(readBuff, 0, 512, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket连接失败 " + ex.ToString());
            }
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int count = socket.EndReceive(ar);
                string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
                msgList.Add(recvStr);
                socket.BeginReceive(readBuff, 0, 512, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket接收失败 " + ex.ToString());
            }
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int count = socket.EndSend(ar);
                Debug.Log("发送成功 " + count);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket发送失败 " + ex.ToString());
            }
        }
    }
}