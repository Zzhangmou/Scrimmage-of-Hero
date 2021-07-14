using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// ���������
    /// </summary>
    public static class NetManager
    {
        private static Socket socket;
        //���ջ�����
        private static byte[] readBuff = new byte[512];
        /// <summary>
        /// ί������
        /// </summary>
        /// <param name="str"></param>
        public delegate void MsgListener(string str);
        //�����б�
        private static Dictionary<string, MsgListener> listeners = new Dictionary<string, MsgListener>();
        //��Ϣ�б�
        private static List<string> msgList = new List<string>();
        /// <summary>
        /// ��Ӽ���
        /// </summary>
        /// <param name="msgName">��������</param>
        /// <param name="listener">�����¼�</param>
        public static void AddListener(string msgName, MsgListener listener)
        {
            listeners[msgName] = listener;
        }
        /// <summary>
        /// ��ȡ����
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
            string msgName = split[0];//Э����
            string msgArgs = split[1];//Э����

            //�����ص�
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
                Debug.Log("Socket����ʧ�� " + ex.ToString());
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
                Debug.Log("Socket����ʧ�� " + ex.ToString());
            }
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int count = socket.EndSend(ar);
                Debug.Log("���ͳɹ� " + count);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket����ʧ�� " + ex.ToString());
            }
        }
    }
}