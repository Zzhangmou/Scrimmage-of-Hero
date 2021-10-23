using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkFK
{
    /// <summary>
    /// 字节数组类
    /// </summary>
    public class ByteArray
    {
        //默认大小
        const int DEFAULT_SIZE = 1024;
        //初始大小
        private int initSize = 0;
        //缓冲区
        public byte[] bytes;
        //读写位置
        public int readIndex = 0;
        public int writeIndex = 0;
        //容量
        private int capacity = 0;
        //剩余空间
        public int Remain { get { return capacity - writeIndex; } }
        //数据长度
        public int Length { get { return writeIndex - readIndex; } }

        public ByteArray(int size = DEFAULT_SIZE)
        {
            bytes = new byte[size];
            capacity = size;
            initSize = size;
            readIndex = 0;
            writeIndex = 0;
        }
        public ByteArray(byte[] defaultBytes)
        {
            bytes = defaultBytes;
            capacity = defaultBytes.Length;
            initSize = defaultBytes.Length;
            readIndex = 0;
            writeIndex = defaultBytes.Length;
        }
        /// <summary>
        /// 重设尺寸
        /// </summary>
        /// <param name="size"></param>
        public void ResetSize(int size)
        {
            if (size < Length) return;
            if (size < initSize) return;
            int n = 1; ;
            while (n < size)
                n *= 2;
            capacity = n;
            byte[] newBytes = new byte[capacity];
            Array.Copy(bytes, readIndex, newBytes, 0, writeIndex - readIndex);
            bytes = newBytes;
            writeIndex = Length;
            readIndex = 0;
        }
        /// <summary>
        /// 检查并移动数据
        /// </summary>
        public void CheckAndMoveBytes()
        {
            if (Length < 64)
                MoveBytes();
        }
        /// <summary>
        /// 移动数据
        /// </summary>
        public void MoveBytes()
        {
            if (Length > 0)
                Array.Copy(bytes, readIndex, bytes, 0, Length);
            writeIndex = Length;
            readIndex = 0;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="bs">数组</param>
        /// <param name="offset">开始放入位置</param>
        /// <param name="count">数据长度</param>
        /// <returns></returns>
        public int Write(byte[] bs, int offset, int count)
        {
            if (Remain < count)
                ResetSize(Length + count);
            Array.Copy(bs, offset, bytes, writeIndex, count);
            writeIndex += count;
            return count;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(byte[] bs, int offset, int count)
        {
            count = Math.Min(count, Length);
            Array.Copy(bytes, readIndex, bs, offset, count);
            readIndex += count;
            CheckAndMoveBytes();
            return count;
        }
        public Int16 ReadInt16()
        {
            if (Length < 2) return 0;
            Int16 ret = (Int16)((bytes[readIndex + 1] << 8) | bytes[readIndex]);
            readIndex += 2;
            CheckAndMoveBytes();
            return ret;
        }
        public Int32 ReadInt32()
        {
            if (Length < 4) return 0;
            Int32 ret = (Int32)((bytes[readIndex + 3] << 24) |
                (bytes[readIndex + 2]) << 16 |
                (bytes[readIndex + 1]) << 8 | bytes[readIndex]);
            readIndex += 4;
            CheckAndMoveBytes();
            return ret;
        }
    }
}