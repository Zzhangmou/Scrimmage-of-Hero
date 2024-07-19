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
    /// ���������
    /// </summary>
    public static class NetManager
    {
        private static Socket socket;
        //�Ƿ���������
        private static bool isConnecting = false;
        //�Ƿ����ڹر�
        private static bool isClosing = false;
        //���ջ�����
        private static ByteArray readBuff;
        //д�����
        private static Queue<ByteArray> writeQueue;
        /// <summary>
        /// �¼�ί������
        /// </summary>
        /// <param name="str"></param>
        public delegate void EventListener(string str);
        //�¼������б�
        private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();
        /// <summary>
        /// ��Ϣί������
        /// </summary>
        /// <param name="msgBase"></param>
        public delegate void MsgListener(ProtoBuf.IExtensible msgBase);
        //��Ϣ�����б�
        private static Dictionary<string, MsgListener> msgListeners = new Dictionary<string, MsgListener>();
        //��Ϣ�б�
        private static List<ProtoBuf.IExtensible> msgList;
        //��Ϣ�б���
        private static int msgCount = 0;
        //ÿ��Update�������Ϣ��
        readonly static int MAX_MESSAGE_FIRE = 20;
        //�Ƿ���������
        public static bool isUserPing = true;
        //�������ʱ��
        public static int pingInterval = 20;
        //���������ʱ����
        private static int closeIntervalMultiple = 4;
        //��һ�η���Pingʱ��
        private static float lastPingTime = 0;
        //��һ���յ�Pongʱ��
        private static float lastPongTime = 0;
        //�¼�
        public enum NetEvent
        {
            ConnectSucc = 1,
            ConnectFail = 2,
            Close = 3
        }
        /// <summary>
        /// �¼���Ӽ���
        /// </summary>
        /// <param name="msgName">��������</param>
        /// <param name="listener">�����¼�</param>
        public static void AddEventListener(NetEvent netEvent, EventListener listener)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent] += listener;
            else
                eventListeners[netEvent] = listener;
        }
        /// <summary>
        /// �¼��Ƴ�����
        /// </summary>
        /// <param name="netEvent">�¼�</param>
        /// <param name="listener">����</param>
        public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent] -= listener;
            if (eventListeners[netEvent] == null)
                eventListeners.Remove(netEvent);
        }
        /// <summary>
        /// �ַ��¼�
        /// </summary>
        /// <param name="netEvent">�¼���</param>
        /// <param name="err">���ݵ���Ϣ</param>
        private static void FireEvent(NetEvent netEvent, string err)
        {
            if (eventListeners.ContainsKey(netEvent))
                eventListeners[netEvent](err);
        }
        /// <summary>
        /// �����Ϣ����
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
        /// �Ƴ���Ϣ����
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
        /// �ַ���Ϣ
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="msgBase"></param>
        private static void FireMsg(string msgName, ProtoBuf.IExtensible msgBase)
        {
            //����Э������
            string name = msgName.ToString().Replace("proto.", "");
            Debug.Log("�յ� " + name);
            if (msgListeners.ContainsKey(name))
                msgListeners[name](msgBase);
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
            if (socket != null && socket.Connected)
            {
                Debug.Log("����ʧ�ܣ��Ѿ�������");
                return;
            }
            if (isConnecting)
            {
                Debug.Log("����ʧ�ܣ���������");
                return;
            }
            InitState();
            socket.NoDelay = true;
            isConnecting = true;
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }
        /// <summary>
        /// ��ʼ��״̬
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
            //����PongЭ��
            if (!msgListeners.ContainsKey("MsgPong"))
                AddMsgListener("MsgPong", OnMsgPong);
        }
        public static void Close()
        {
            //״̬�ж�
            if (socket == null || !socket.Connected) return;
            if (isConnecting) return;
            if (writeQueue.Count > 0)//������������ڷ���
                isClosing = true;
            else
            {
                socket.Close();
                FireEvent(NetEvent.Close, "");
            }
        }
        public static void Send(ProtoBuf.IExtensible msgBase)
        {
            //״̬�ж�
            if (socket == null || !socket.Connected) return;
            if (isConnecting) return;
            Debug.Log("2");
            if (isClosing) return;


            byte[] nameBytes = ProtobufHelper.EncodeName(msgBase);
            byte[] bodyBytes = ProtobufHelper.Encode(msgBase);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[2 + len];
            //��װ����
            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            //��װ����
            Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
            //��װЭ����
            Array.Copy(bodyBytes, 0, sendBytes, nameBytes.Length + 2, bodyBytes.Length);

            //д�����
            ByteArray ba = new ByteArray(sendBytes);
            int count = 0;//writeQueue�ĳ���
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
        /// ������Ϣ
        /// </summary>
        private static void MsgUpdate()
        {
            if (msgCount == 0) return;
            //������Ϣ
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
                //�ַ���Ϣ
                if (msgBase != null)
                    FireMsg(msgBase.ToString(), msgBase);
                else//û����Ϣ��
                    break;
            }
        }
        /// <summary>
        /// ����PingЭ��
        /// </summary>
        private static void PingUpdate()
        {
            if (!isUserPing) return;
            //����Ping
            if (Time.time - lastPingTime > pingInterval)
            {
                MsgPing msgPing = new MsgPing();
                Send(msgPing);
                lastPingTime = Time.time;
                Debug.Log("����Ping");
            }
            //���Pongʱ��
            if (Time.time - lastPongTime > pingInterval * closeIntervalMultiple)
            {
                Debug.Log("Pong�ظ���ʱ");
                Close();
            }
        }
        /// <summary>
        /// ����PongЭ��
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
                Debug.Log("���ӳɹ�");
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>CallLuaHelper.PanelShow("TipPanel", "���ӳɹ�"));
                FireEvent(NetEvent.ConnectSucc, "");
                isConnecting = false;
                //��ʼ����
                socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.Remain, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket����ʧ�� " + ex.ToString());
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() => CallLuaHelper.PanelShow("TipPanel", "Socket����ʧ�� " + ex.ToString()));
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
                //�����������Ϣ
                OnReceiveData();
                //������������
                if (readBuff.Remain < 8)
                {
                    readBuff.MoveBytes();
                    readBuff.ResetSize(readBuff.Length * 2);
                }
                socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.Remain, 0, ReceiveCallback, socket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket����ʧ�� " + ex.ToString());
            }
        }

        private static void OnReceiveData()
        {
            //�ж���Ϣ����
            if (readBuff.Length <= 2) return;
            //�����Ϣ�峤��
            int readIndex = readBuff.readIndex;
            byte[] bytes = readBuff.bytes;
            Int16 bodyLength = (Int16)((bytes[readIndex + 1] << 8 | bytes[readIndex]));
            //�жϳ���
            if (readBuff.Length < bodyLength + 2) return;
            readBuff.readIndex += 2;
            //����Э����
            int nameCount = 0;
            string protoName = ProtobufHelper.DecodeName(readBuff.bytes, readBuff.readIndex, out nameCount);
            if (protoName == "")
            {
                Debug.Log("����Э����ʧ��,Ϊ��");
                return;
            }
            Debug.Log(protoName);
            readBuff.readIndex += nameCount;
            //����Э����
            int bodyCount = bodyLength - nameCount;
            ProtoBuf.IExtensible msgBase = ProtobufHelper.Decode(protoName, readBuff.bytes, readBuff.readIndex, bodyCount);
            readBuff.readIndex += bodyCount;
            readBuff.CheckAndMoveBytes();
            //��ӵ���Ϣ�ж�
            lock (msgList)
            {
                msgList.Add(msgBase);
            }
            msgCount++;
            //������ȡ��Ϣ
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
                //��������
                ba.readIndex += count;
                if (ba.Length == 0)
                {
                    lock (writeQueue)
                    {
                        writeQueue.Dequeue();
                        ba = writeQueue.First();
                    }
                }
                //��������
                if (ba != null)
                    socket.BeginSend(ba.bytes, ba.readIndex, ba.Length, 0, SendCallback, socket);
                else if (isClosing)
                    socket.Close();
                Debug.Log("ok");
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket����ʧ�� " + ex.ToString());
            }
        }
    }
}