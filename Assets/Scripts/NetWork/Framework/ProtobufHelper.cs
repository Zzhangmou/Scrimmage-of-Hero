using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Protobuf解析类
    /// </summary>
    public static class ProtobufHelper
    {
        //编码
        public static byte[] Encode(ProtoBuf.IExtensible msgBase)
        {
            using var memory = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize(memory, msgBase);
            return memory.ToArray();
        }
        //解码
        public static ProtoBuf.IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
        {
            using var memory = new System.IO.MemoryStream(bytes, offset, count);
            //添加命名空间
            System.Type t = System.Type.GetType("proto." + protoName);
            return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
        }
        //编码协议名
        public static byte[] EncodeName(ProtoBuf.IExtensible msgBase)
        {
            //去掉命名空间信息  proto.
            string str = msgBase.ToString().Replace("proto.", "");
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(str.ToString());
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
        //解析协议名
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

