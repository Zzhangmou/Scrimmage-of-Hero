using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Protobuf������
    /// </summary>
    public static class ProtobufHelper
    {
        //����
        public static byte[] Encode(ProtoBuf.IExtensible msgBase)
        {
            using var memory = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize(memory, msgBase);
            return memory.ToArray();
        }
        //����
        public static ProtoBuf.IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
        {
            using var memory = new System.IO.MemoryStream(bytes, offset, count);
            protoName = "proto." + protoName;//���������ռ�
            System.Type t = System.Type.GetType(protoName);
            return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
        }
        //����Э����
        public static byte[] EncodeName(ProtoBuf.IExtensible msgBase)
        {
            //ȥ�������ռ���Ϣ  proto.
            string str = msgBase.ToString().Replace("proto.", "");
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(str.ToString());
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
        //����Э����
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
