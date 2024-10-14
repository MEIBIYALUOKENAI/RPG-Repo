using SimpleServer.Bussiness;
using SimpleServer.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Proto
{
    public class MsgHandler
    {
        public static void MsgSecret(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgSecret msgSecret = (MsgSecret)msgBase;
            msgSecret.Secret = ServerSocket.SecretKey;
            ServerSocket.Send(clientSocket, msgSecret);
            Debug.Log("向客户端推送密钥消息：" + msgSecret.Secret);
        }
        public static void MsgPing(ClientSocket clientSocket, MsgBase msgBase)
        {
            clientSocket.LastPingTime = ServerSocket.GetTimeStamp();//服务端记录当前客户端发送心跳包的时间
            MsgPing msg = new MsgPing();
            ServerSocket.Send(clientSocket, msg);
            Debug.Log("向客户端推送心跳包");
        }
        public static void MsgTest(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgTest msgTest = (MsgTest)msgBase;
            Debug.Log("收到客户端的信息：" + msgTest.ReqContent);
            msgTest.RecContent = "server向client发送数据！！！！";
            ServerSocket.Send(clientSocket, msgTest);
        }
        //处理注册
        public static void MsgRegister(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgRegister msg = (MsgRegister)msgBase;
            RegisterResult registerResult = UserManager.Instance.Register(msg.registerType, msg.Account, msg.Password);
            msg.registerResult = registerResult;
            ServerSocket.Send(clientSocket, msg);
        }
        //登录
        public static void MsgLogin(ClientSocket clientSocket,MsgBase msgBase)
        {
            MsgLogin msg = (MsgLogin)msgBase;
            string token;
            LoginResult loginResult = UserManager.Instance.Login(msg.loginType, msg.Account, msg.Password, out token);
            msg.loginResult = loginResult;
            msg.token = token;
            ServerSocket.Send(clientSocket, msg);
            //ServerSocket.Send(clientSocket, role);
        }


        public static void MsgUserRoles(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgUserRoles msg = (MsgUserRoles)msgBase;

            msg= UserManager.Instance.QueryByUsername(msg.Username);
            ServerSocket.Send(clientSocket, msg);
            //ServerSocket.Send(clientSocket, role);
        }

        

        //添加物品
        public static void MsgAddBagItem(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgAddBagItem msg = (MsgAddBagItem)msgBase;
            UserManager.Instance.AddBagItem(msg);
        }
        //删除物品
        public static void MsgDeleteBagItem(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgDeleteBagItem msg = (MsgDeleteBagItem)msgBase;
            UserManager.Instance.DeleteBagItem(msg);
        }
        //更新物品 穿戴、等级。。。。。
        public static void MsgUpdateBagItem(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgUpdateBagItem msg = (MsgUpdateBagItem)msgBase;
            UserManager.Instance.UpdateBagItem(msg);
        }
        //新增学习的技能
        public static void MsgStudyFun(ClientSocket clientSocket, MsgBase msgBase) {
            MsgStudyFun msg = (MsgStudyFun)msgBase;
            UserManager.Instance.StudyFun(msg);
        }
        //更新技能数据
        public static void MsgUpFun(ClientSocket clientSocket, MsgBase msgBase) {
            MsgUpFun msg = (MsgUpFun)msgBase;
            UserManager.Instance.UpFun(msg);
        }
        //新增接取的任务
        public static void MsgAddTask(ClientSocket clientSocket, MsgBase msgBase) {
            MsgAddTask msg = (MsgAddTask)msgBase;
            UserManager.Instance.AddTask(msg);
        }
        //更新任务数据
        public static void MsgUpdateTask(ClientSocket clientSocket, MsgBase msgBase) {
            MsgUpdateTask msg = (MsgUpdateTask)msgBase;
            UserManager.Instance.UpdateTask(msg);
        }

        //完成任务需要删除
        public static void MsgDeleteTask(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgDeleteTask msg = (MsgDeleteTask)msgBase;
            UserManager.Instance.DeleteTask(msg);
        }

        public static void MsgUpdateUserRole(ClientSocket clientSocket, MsgBase msgBase)
        {
            MsgUpdateUserRole msg = (MsgUpdateUserRole)msgBase;
            UserManager.Instance.UpdateUserRole(msg);
        }
    }
}
