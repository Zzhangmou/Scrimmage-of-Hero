using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using Common;
using proto;
using Helper;

namespace NetWorkFK
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public static class NetManager
    {
        private static Socket socket;
        //是否正在连接
        private static bool isConnecting = false;
        //是否正在关闭
        private static bool isClosing = false;
        //接收缓冲区
        private static ByteArray readBuff;
        //写入队列
        private static Queue<ByteArray> writeQueue;
        /// <summary>
        /// 事件委托类型
        /// </summary>
        /// <param name="str"></param>
        public delegate void EventListener(string str);
        //事件监听列表
        private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();
        /// <summary>
        /// 消息委托类型
        /// </summary>
        /// <param name="msgBase"></param>
        public delegate void MsgListener(ProtoBuf.IExtensible msgBase);
        //消息监听列表
        private static Dictionary<string, MsgListener> msgListeners = new Dictionary<string, MsgListener>();
        //消息列表
        private static List<ProtoBuf.IExtensible> msgList;
        //消息列表长度
        private static int msgCount = 0;
        //每次Update处理的消息量
        readonly static int MAX_MESSAGE_FIRE = 20;
        //是否启用心跳
        public static bool isUserPing = true;
        //心跳间隔时间
        public static int pingInterval = 20;
        //心跳间隔超时倍数
        private static int closeIntervalMultiple = 4;
        //上一次发送Ping时间
        private static float lastPingTime = 0;
        //上一次收到Pong时间
        private static float lastPongTime = 0;
        //事件
        public enum NetEvent
        {
            ConnectSucc = 1,
            ConnectFail = 2,
            Close = 3
        }
        /// <summary>
        /// 事件添加监听
        /// </summary>
        /// <param name="msgName">监听名称</param>
        /// <param name="listener">监听事件</param>
        public static void AddEventListener(NetEvent netEvent, EventListener listener)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent] += listener;
            else
                eventListeners[netEvent] = listener;
        }
        /// <summary>
        /// 事件移除监听
        /// </summary>
        /// <param name="netEvent">事件</param>
        /// <param name="listener">监听</param>
        public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent] -= listener;
            if (eventListeners[netEvent] == null)
                eventListeners.Remove(netEvent);
        }
        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="netEvent">事件名</param>
        /// <param name="err">传递的信息</param>
        private static void FireEvent(NetEvent netEvent, string err)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent](err);
        }
        /// <summary>
        /// 添加消息监听
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="listener"></param>
        public static void AddMsgListener(string msgName, MsgListener listener)
        {
            if (msgListeners.ContainsKey(msgName))
                msgListeners[msgName] += listener;
            else
                msgListeners[msgName] = listener;
        }
        /// <summary>
        /// 移除消息监听
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="listener"></param>
        public static void RemoveMsgListener(string msgName, MsgListener listener)
        {
            if (msgListeners.ContainsKey(msgName))
                msgListeners[msgName] -= listener;
            if (msgListeners[msgName] == null)
                msgListeners.Remove(msgName);
        }
        /// <summary>
        /// 分发消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="msgBase"></param>
        private static void FireMsg(string msgName, ProtoBuf.IExtensible msgBase)
        {
            //处理协议名称
            string name = msgName.ToString().Replace("proto.", "");
            Debug.Log("收到 " + name);
            if (msgListeners.ContainsKey(name))
                msgListeners[name](msgBase);
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
            if (socket != null && socket.Connected)
            {
                Debug.Log("连接失败，已经连接了");
                return;
            }
            if (isConnecting)
            {
                Debug.Log("连接失败，正在连接");
                return;
            }
            InitState();
            socket.NoDelay = true;
            isConnecting = true;
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }
        /// <summary>
        /// 初始化状态
        /// </summary>
        private static void InitState()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            readBuff = new ByteArray();
            writeQueue = new Queue<ByteArray>();
            isConnecting = false;
            isClosing = false;
            msgList = new List<ProtoBuf.IExtensible>();
            msgCount = 0;
            lastPingTime = Time.time;
            lastPongTime = Time.time;
            //监听Pong协议
            if (!msgListeners.ContainsKey("MsgPong"))
                AddMsgListener("MsgPong", OnMsgPong);
        }
        public static void Close()
        {
            //状态判断
            if (socket == null || !socket.Connected) return;
            if (isConnecting) return;
            if (writeQueue.Count > 0)//如果还有数据在发送
                isClosing = true;
            else
            {
                socket.Close();
                FireEvent(NetEvent.Close, "");
            }
        }
        public static void Send(ProtoBuf.IExtensible msgBase)
        {
            //状态判断
            if (socket == null || !socket.Connected) return;
            if (isConnecting) return;
            Debug.Log("2");
            if (isClosing) return;


            byte[] nameBytes = ProtobufHelper.EncodeName(msgBase);
            byte[] bodyBytes = ProtobufHelper.Encode(msgBase);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[2 + len];
            //组装长度
            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            //组装名字
            Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
            //组装协议体
            Array.Copy(bodyBytes, 0, sendBytes, nameBytes.Length + 2, bodyBytes.Length);

            //写入队列
            ByteArray ba = new ByteArray(sendBytes);
            int count = 0;//writeQueue的长度
            lock (writeQueue)
            {
                writeQueue.Enqueue(ba);
                count = writeQueue.Count;
            }
            if (count == 1)
            {
                socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
            }
        }

        public static void Update()
        {
            MsgUpdate();
            PingUpdate();
        }
        /// <summary>
        /// 更新消息
        /// </summary>
        private static void MsgUpdate()
        {
            if (msgCount == 0) return;
            //处理消息
            for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
            {
                ProtoBuf.IExtensible msgBase = null;
                lock (msgList)
                {
                    if (msgCount > 0)
                    {
                        msgBase = msgList[0];
                        msgList.RemoveAt(0);
                        msgCount--;
                    }
                }
                //分发消息
                if (msgBase != null)
                    FireMsg(msgBase.ToString(), msgBase);
                else//没有消息了
                    break;
            }
        }
        /// <summary>
        /// 发送Ping协议
        /// </summary>
        private static void PingUpdate()
        {
            if (!isUserPing) return;
            //发送Ping
            if (Time.time - lastPingTime > pingInterval)
            {
                MsgPing msgPing = new MsgPing();
                Send(msgPing);
                lastPingTime = Time.time;
                Debug.Log("发送Ping");
            }
            //检测Pong时间
            if (Time.time - lastPongTime > pingInterval * closeIntervalMultiple)
            {
                Debug.Log("Pong回复超时");
                Close();
            }
        }
        /// <summary>
        /// 监听Pong协议
        /// </summary>
        /// <param name="msgBase"></param>
        private static void OnMsgPong(ProtoBuf.IExtensible msgBase)
        {
            lastPongTime = Time.time;
        }
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                Debug.Log("连接成功");
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>CallLuaHelper.PanelShow("TipPanel", "连接成功"));
                FireEvent(NetEvent.ConnectSucc, "");
                isConnecting = false;
                //开始接收
                socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.Remain, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket连接失败 " + ex.ToString());
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() => CallLuaHelper.PanelShow("TipPanel", "Socket连接失败 " + ex.ToString()));
                FireEvent(NetEvent.ConnectFail, ex.ToString());
                isConnecting = false;
            }
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int count = socket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                    return;
                }
                readBuff.writeIndex += count;
                //处理二进制消息
                OnReceiveData();
                //继续接收数据
                if (readBuff.Remain < 8)
                {
                    readBuff.MoveBytes();
                    readBuff.ResetSize(readBuff.Length * 2);
                }
                socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.Remain, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket接收失败 " + ex.ToString());
            }
        }

        private static void OnReceiveData()
        {
            //判断消息长度
            if (readBuff.Length <= 2) return;
            //获得消息体长度
            int readIndex = readBuff.readIndex;
            byte[] bytes = readBuff.bytes;
            Int16 bodyLength = (Int16)((bytes[readIndex + 1] << 8 | bytes[readIndex]));
            //判断长度
            if (readBuff.Length < bodyLength + 2) return;
            readBuff.readIndex += 2;
            //解析协议名
            int nameCount = 0;
            string protoName = ProtobufHelper.DecodeName(readBuff.bytes, readBuff.readIndex, out nameCount);
            if (protoName == "")
            {
                Debug.Log("解析协议名失败,为空");
                return;
            }
            Debug.Log(protoName);
            readBuff.readIndex += nameCount;
            //解析协议体
            int bodyCount = bodyLength - nameCount;
            ProtoBuf.IExtensible msgBase = ProtobufHelper.Decode(protoName, readBuff.bytes, readBuff.readIndex, bodyCount);
            readBuff.readIndex += bodyCount;
            readBuff.CheckAndMoveBytes();
            //添加到消息列队
            lock (msgList)
            {
                msgList.Add(msgBase);
            }
            msgCount++;
            //继续读取消息
            if (readBuff.Length > 2)
                OnReceiveData();
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Debug.Log("1");
                Socket socket = (Socket)ar.AsyncState;
                if (socket == null && !socket.Connected) return;

                int count = socket.EndSend(ar);

                ByteArray ba;
                lock (writeQueue)
                {
                    ba = writeQueue.First();
                }
                //完整发送
                ba.readIndex += count;
                if (ba.Length == 0)
                {
                    lock (writeQueue)
                    {
                        writeQueue.Dequeue();
                        ba = writeQueue.First();
                    }
                }
                //继续发送
                if (ba != null)
                    socket.BeginSend(ba.bytes, ba.readIndex, ba.Length, 0, SendCallback, socket);
                else if (isClosing)
                    socket.Close();
                Debug.Log("ok");
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket发送失败 " + ex.ToString());
            }
        }
    }
}