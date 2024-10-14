using SimpleServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
/*
 * 公钥：
 * 密钥：
 * 分包：当数据比较大的时候，一次性发送不过去，底层会自动将其分成几个数据包发送
 * 粘包：当数据比较小的时候，底层会自动进行一个合并，合并成一个大数据包进行发送
 */
namespace SimpleServer.Net
{
    public class ServerSocket : Singleton<ServerSocket>
    {
        public static string PublicKey = "abc1234";//公钥
        public static string SecretKey = "123abcd";//密钥
#if DEBUG
        private string m_IPStr = "127.0.0.1";
#else
        private string m_IPStr = "172.24.239.253";//阿里云的内网地址
#endif
        private const int m_Port = 8999;
        private static Socket m_ListenSocket;
        private static List<Socket> m_checkReadList = new List<Socket>();//存储所有客户端的socket
        private static Dictionary<Socket, ClientSocket> m_ClientDic = new Dictionary<Socket, ClientSocket>();//存储所有链接上的客户端
        public static long m_PingInterval = 100;//心跳包间隔时间
        public static List<ClientSocket> m_TempList = new List<ClientSocket>();//存储所有心跳包超时的客户端
        //初始化
        public void Init()
        {
            //绑定ip地址和端口号，监听客户端的连接
            IPAddress iP = IPAddress.Parse(m_IPStr);
            IPEndPoint iPEndPoint = new IPEndPoint(iP, m_Port);
            m_ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ListenSocket.Bind(iPEndPoint);
            m_ListenSocket.Listen(500);
            Debug.LogInfo("服务器启动监听{0}成功！", m_ListenSocket.LocalEndPoint.ToString());

            //循环处理客户端socket的连接请求和数据处理
            while (true)
            {
                ResetCheckRead();
                try
                {
                    Socket.Select(m_checkReadList, null, null, 1000);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                //为什么可以先处理发送数据的，而不是请求连接的
                //因为请求连接的客户端socket会与监听的一致，而监听的socket在头部
                for (int i = m_checkReadList.Count - 1; i >= 0; i--)
                {
                    Socket s = m_checkReadList[i];
                    //建立连接
                    if (s == m_ListenSocket)
                    {
                        //处理连接
                        ReadListen(s);
                    }
                    //带数据的socket处理
                    else
                    {
                        ReadClient(s);
                    }
                }
                //检测心跳包是否超时
                long timeNow = GetTimeStamp();
                m_TempList.Clear();

                //将心跳包超时的客户端socket加入待清除list中
                foreach (ClientSocket clientSocket in m_ClientDic.Values)
                {
                    if (timeNow - clientSocket.LastPingTime > m_PingInterval * 2)
                    {
                        Debug.Log("心跳包超时！" + clientSocket.socket.RemoteEndPoint);
                        m_TempList.Add(clientSocket);
                    }
                }
                //统一关闭所有心跳包超时的客户端
                foreach (ClientSocket item in m_TempList)
                {
                    CloseSocket(item);
                }
            }
        }

        //检测哪些socket是可以读取的
        public void ResetCheckRead()
        {
            m_checkReadList.Clear();
            //首先添加监听的socket
            m_checkReadList.Add(m_ListenSocket);
            //其次添加数据处理的客户端socket
            foreach (Socket s in m_ClientDic.Keys)
            {
                m_checkReadList.Add(s);
            }
        }

        //建立连接
        void ReadListen(Socket listen)
        {
            try
            {
                Socket client = listen.Accept();
                ClientSocket clientSocket = new ClientSocket();
                clientSocket.socket = client;
                clientSocket.LastPingTime = GetTimeStamp();
                //加入已连接socket字典
                m_ClientDic.Add(client, clientSocket);
                Debug.Log("一个客户端链接{0}，当前有{1}个客户端在线！", client.LocalEndPoint.ToString(), m_ClientDic.Count);
            }
            catch (SocketException e)
            {
                Debug.LogError("建立连接失败！" + e);
            }
        }

        //处理客户端的数据
        void ReadClient(Socket client)
        {
            ClientSocket clientSocket = m_ClientDic[client];
            ByteArray byteArray = clientSocket.byteArray;//获取对应客户端的数据缓冲区
            int count = 0;
            //该客户端socket的数据缓冲区没有空间了
            if (byteArray.Remain <= 0)
            {
                //数据接收，尝试
                OnReceiveData(clientSocket);
                byteArray.CheckAndMoveBytes();
                //清楚冗余数据后发现还是不够，扩容
                while (byteArray.Remain <= 0)
                {
                    int expandSize = byteArray.Length < ByteArray.defauleSize ? ByteArray.defauleSize : byteArray.Length;
                    byteArray.ReSize(expandSize * 2);
                }
            }
            //接收客户端的数据
            try
            {
                //使用我还有的空间接收数据 [writeIndex,writeIndex+remain]
                count = client.Receive(byteArray.Bytes, byteArray.WriteIndex, byteArray.Remain, 0);
            }
            catch (SocketException e)
            {
                Debug.LogError("接收信息失败！" + e);
                CloseSocket(clientSocket);
                return;
                throw;
            }
            if (count <= 0)
            {
                CloseSocket(clientSocket);
                return;
            }
            //写完偏移
            byteArray.WriteIndex += count;
            OnReceiveData(clientSocket);
            byteArray.CheckAndMoveBytes();
        }
        //对接收到的数据进行处理
        void OnReceiveData(ClientSocket clientSocket)
        {
            ByteArray byteArray = clientSocket.byteArray;
            //数据有问题吧
            if (byteArray.Length <= 4 || byteArray.ReadIndex < 0)
            {
                return;
            }
            int readIdx = byteArray.ReadIndex;
            byte[] bytes = byteArray.Bytes;
            //返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数
            //因为，我们约定，readIdx前四个存储数据的大小
            int bodyLength = BitConverter.ToInt32(bytes, readIdx);
            //实际上的数据存储并没有这么多，说明产生了分包
            if (byteArray.Length < bodyLength + 4)//分包
            {
                return;
            }
            //对接收到的完整的消息进行处理
            byteArray.ReadIndex += 4;
            //解析协议名
            int nameCount = 0;
            ProtocolEnum protocol = ProtocolEnum.None;
            try
            {
                //获取解析后的协议名
                protocol = MsgBase.DecodeName(byteArray.Bytes, byteArray.ReadIndex, out nameCount);
            }
            catch (Exception e)
            {
                Debug.LogError("解析协议名异常！" + e);
                CloseSocket(clientSocket);
                return;
                throw;
            }
            if (protocol == ProtocolEnum.None)
            {
                Debug.LogError("DecodeName解析协议名失败！");
                CloseSocket(clientSocket);
                return;
            }
            //协议名解析成功，偏移到数据部分
            byteArray.ReadIndex += nameCount;
            //减去协议名的大小得到数据的大小
            int bodyCount = bodyLength - nameCount;
            //解析协议内容
            MsgBase msgBase = null;
            try
            {
                //解析内容数据
                msgBase = MsgBase.Decode(protocol, byteArray.Bytes, byteArray.ReadIndex, bodyCount);
                if (msgBase == null)
                {
                    Debug.LogError("{0}协议内容解析失败！" + protocol.ToString());
                    CloseSocket(clientSocket);
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("解析协议内容异常！" + e.ToString());
                CloseSocket(clientSocket);
                return;
            }
            //该完整消息解析完毕，跳到下一条消息的解析位置
            byteArray.ReadIndex += bodyCount;
            //清除已处理数据
            byteArray.CheckAndMoveBytes();
            //一条完整的消息解析完成，需要对这条消息进行处理
            Debug.Log("{0}协议消息解析完成，对{1}消息进行分发处理！", protocol.ToString(), protocol.ToString());

            //类都没有实例化怎么拿方法，该方法是静态的属于类！！
            MethodInfo methodInfo = typeof(MsgHandler).GetMethod(protocol.ToString());
            object[] obj = { clientSocket, msgBase };
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, obj);
            }
            else
            {
                Debug.LogError("找不到与该协议对应的函数处理！" + protocol);
            }
            if (byteArray.Length > 4)//粘包
            {
                OnReceiveData(clientSocket);
            }
        }
        //发送数据
        public static void Send(ClientSocket clientSocket, MsgBase msgBase)
        {
            if (clientSocket == null || !clientSocket.socket.Connected)
            {
                return;
            }
            try
            {
                //构建消息 消息头+消息名字+消息内容
                byte[] nameBytes = MsgBase.EncodeName(msgBase);
                byte[] bodyBytes = MsgBase.Encode(msgBase);
                int len = nameBytes.Length + bodyBytes.Length;
                byte[] headBytes = BitConverter.GetBytes(len);
                byte[] sendBytes = new byte[headBytes.Length + len];
                Array.Copy(headBytes, 0, sendBytes, 0, headBytes.Length);
                Array.Copy(nameBytes, 0, sendBytes, headBytes.Length, nameBytes.Length);
                Array.Copy(bodyBytes, 0, sendBytes, headBytes.Length + nameBytes.Length, bodyBytes.Length);
                //发送数据
                try
                {
                    clientSocket.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
                }
                catch (SocketException e)
                {

                    Debug.LogError("发送数据异常！" + e.ToString());
                }
            }
            catch (SocketException e)
            {
                Debug.LogError("构建数据异常！" + e.ToString());
                throw;
            }
        }

        //获取时间戳
        public static long GetTimeStamp()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }
        //关闭连接
        public void CloseSocket(ClientSocket clientSocket)
        {
            clientSocket.socket.Close();
            m_ClientDic.Remove(clientSocket.socket);
            Debug.Log("一个客户端断开连接，当前总连接数{0}", m_ClientDic.Count);
        }
    }
}