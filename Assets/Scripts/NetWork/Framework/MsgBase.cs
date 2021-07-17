using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkFK
{
    /// <summary>
    /// 协议基类
    /// </summary>
    public class MsgBase : MonoBehaviour
    {
        /// <summary>
        /// 协议名
        /// </summary>
        public string protoName = "";
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="msgBase">协议</param>
        /// <returns></returns>
        public static byte[] Encode(MsgBase msgBase)
        {
            string s = JsonUtility.ToJson(msgBase);
            return System.Text.Encoding.UTF8.GetBytes(s);
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="protoName">协议名</param>
        /// <param name="bytes">字节数组</param>
        /// <param name="offset">起始地址</param>
        /// <param name="count">长度</param>
        /// <returns></returns>
        public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
        {
            string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
            return (MsgBase)JsonUtility.FromJson(s, Type.GetType(protoName));
        }
        /// <summary>
        /// 编码协议名(2字节长度+字符串)
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] EncodeName(MsgBase msgBase)
        {
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
            Int16 len = (Int16)nameBytes.Length;
            //申请bytes数组
            byte[] bytes = new byte[2 + len];
            //组装2字节的长度信息
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);
            //组装名字
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }
        /// <summary>
        /// 解析协议名
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="count">长度</param>
        /// <returns></returns>
        public static string DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            //必须大于两字节
            if (offset + 2 > bytes.Length) return "";
            //读取长度
            Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
            //检查长度
            if (len < 0) return "";
            //检查长度
            if (offset + 2 + len > bytes.Length) return "";
            //解析
            count = 2 + len;
            string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
            return name;
        }
    }
}

