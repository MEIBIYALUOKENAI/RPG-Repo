
public enum ProtocolEnum
{
    None = 0,
    MsgSecret = 1,//密钥协议
    MsgPing = 2,//心跳包协议
    MsgRegister = 3,//注册协议
    MsgLogin = 4,//登录协议
    MsgRole = 5,
    MsgUserRoles=6,
    MsgUpdateRole=7,
    MsgAddBagItem=8,
    MsgDeleteBagItem =9,
    MsgStudyFun=10,
    MsgUpFun=11,
    MsgAddTask=12,
    MsgUpdateTask=13,
    MsgDeleteTask=14,
    MsgUpdateBagItem = 15,
    MsgUpdateUserRole=16,


    MsgTest =999,//测试分包粘包的协议
}
