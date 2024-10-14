using ProtoBuf;
using SimpleServer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 消息：消息头（消息名字的长度+消息内容的长度）+消息名字+消息内容
 */
namespace SimpleServer.Proto
{
    public class MsgBase
    {
        public virtual ProtocolEnum ProtocolType { get; set; }
        //协议内容序列化和加密
        public static byte[] Encode(MsgBase msgBase)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                Serializer.Serialize(memory, msgBase);
                byte[] bytes = memory.ToArray();
                string secret = ServerSocket.SecretKey;
                if (msgBase is MsgSecret)
                {
                    secret = ServerSocket.PublicKey;
                }
                bytes = AES.AESEncrypt(bytes, secret);
                return bytes;
            }
        }
        //协议内容解密和反序列化
        //                                                                        数据起始位   数据大小
        public static MsgBase Decode(ProtocolEnum protocolEnum, byte[] bytes, int offset, int count)
        {
            if (count <= 0)
            {
                Debug.LogError("协议内容解密出错！数据长度为0");
                return null;
            }
            try
            {
                //拷贝一份
                byte[] newBytes = new byte[count];
                Array.Copy(bytes, offset, newBytes, 0, count);
                //使用什么钥匙解析
                string secret = ServerSocket.SecretKey;
                //如果该消息是请求密钥的，则使用公钥解析
                if (protocolEnum == ProtocolEnum.MsgSecret)
                {
                    secret = ServerSocket.PublicKey;
                }
                //解密，使用公钥
                newBytes = AES.AESDecrypt(newBytes, secret);
                //反序列化
                using (MemoryStream memory = new MemoryStream(newBytes, 0, newBytes.Length))
                {
                    Type t = System.Type.GetType(protocolEnum.ToString());//根据协议的枚举名字获取到协议的类名
                    return (MsgBase)Serializer.NonGeneric.Deserialize(t, memory);

                }
            }
            catch (Exception e)
            {
                Debug.LogError("协议内容解密出错！");
                return null;
            }
        }
        //协议名编码
        public static byte[] EncodeName(MsgBase msgBase)
        {
            //获取协议名字，枚举转字符串，存储到字节数组
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.ProtocolType.ToString());
            //len为协议名的这个名字的数据长度
            Int16 len = (Int16)nameBytes.Length;
            byte[] bytes = new byte[2 + len];
            //将这个16位的数据长度，拆分为两个字节，0存储低8位
            //1存储高8位
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);

            //将协议名这个数据存储到bytes[2]以后,因为下标0,1存储该数据的长度
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }
        //协议名解码
        //offset代表协议名起始位置，[offset,offset+1]这两个下标存储该协议名的长度
        public static ProtocolEnum DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            //协议有问题
            if (offset + 2 > bytes.Length)
            {
                return ProtocolEnum.None;
            }
            //提取出该协议名数据的长度
            Int16 len = (Int16)(bytes[offset + 1] << 8 | bytes[offset]);

            //不合理，因为连数据长度都没有
            if (offset + 2 + len > bytes.Length)
            {
                return ProtocolEnum.None;
            }
            //
            count = 2 + len;
            try
            {
                //获取
                string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
                //利用反射，字符串转枚举
                return (ProtocolEnum)System.Enum.Parse(typeof(ProtocolEnum), name);
            }
            catch (Exception e)
            {
                Debug.LogError("不存在该协议：" + e.ToString());
                return ProtocolEnum.None;
            }
        }
    }
}
