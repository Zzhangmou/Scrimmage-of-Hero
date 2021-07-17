using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkFK
{
    /// <summary>
    /// Э�����
    /// </summary>
    public class MsgBase : MonoBehaviour
    {
        /// <summary>
        /// Э����
        /// </summary>
        public string protoName = "";
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="msgBase">Э��</param>
        /// <returns></returns>
        public static byte[] Encode(MsgBase msgBase)
        {
            string s = JsonUtility.ToJson(msgBase);
            return System.Text.Encoding.UTF8.GetBytes(s);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="protoName">Э����</param>
        /// <param name="bytes">�ֽ�����</param>
        /// <param name="offset">��ʼ��ַ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
        {
            string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
            return (MsgBase)JsonUtility.FromJson(s, Type.GetType(protoName));
        }
        /// <summary>
        /// ����Э����(2�ֽڳ���+�ַ���)
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] EncodeName(MsgBase msgBase)
        {
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
            Int16 len = (Int16)nameBytes.Length;
            //����bytes����
            byte[] bytes = new byte[2 + len];
            //��װ2�ֽڵĳ�����Ϣ
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);
            //��װ����
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }
        /// <summary>
        /// ����Э����
        /// </summary>
        /// <param name="bytes">�ֽ�����</param>
        /// <param name="offset">��ʼλ��</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public static string DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            //����������ֽ�
            if (offset + 2 > bytes.Length) return "";
            //��ȡ����
            Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
            //��鳤��
            if (len < 0) return "";
            //��鳤��
            if (offset + 2 + len > bytes.Length) return "";
            //����
            count = 2 + len;
            string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
            return name;
        }
    }
}

