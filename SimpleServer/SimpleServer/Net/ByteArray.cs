using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Net
{
    public class ByteArray
    {
        //缓冲区
        public byte[] Bytes;
        //默认大小
        public const int defauleSize = 1024;
        //初始大小
        private int initSize;
        //读写位置
        public int ReadIndex;
        public int WriteIndex;
        //容量
        private int Capacity;
        //剩余容量
        public int Remain { get { return Capacity - WriteIndex; } }
        //数据长度
        public int Length { get { return WriteIndex - ReadIndex; } }

        public ByteArray()
        {
            Bytes = new byte[defauleSize];
            Capacity = defauleSize;
            initSize = defauleSize;
            ReadIndex = 0;
            WriteIndex = 0;
        }
        //检测并移动数据
        public void CheckAndMoveBytes()
        {
            if (Length < 8)
            {
                if (ReadIndex < 0)
                {
                    return;
                }
                Array.Copy(Bytes, ReadIndex, Bytes, 0, Length);
                WriteIndex = Length;
                ReadIndex = 0;
            }
        }
        //数据扩容
        public void ReSize(int size)
        {
            if (ReadIndex < 0)
            {
                return;
            }
            if (size < Length)
            {
                return;
            }
            if (size < initSize)
            {
                return;
            }
            int n = defauleSize;
            while (n < size)
            {
                n *= 2;
            }
            Capacity = n;//更新容量
            byte[] newBytes = new byte[Capacity];
            Array.Copy(Bytes, ReadIndex, newBytes, 0, Length);
            Bytes = newBytes;
            WriteIndex = Length;
            ReadIndex = 0;
        }
    }
}
