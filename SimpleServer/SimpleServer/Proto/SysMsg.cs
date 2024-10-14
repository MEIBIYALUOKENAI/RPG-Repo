using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using SimpleServer.Proto;

[ProtoContract]
public class MsgSecret : MsgBase
{
    public MsgSecret()
    {
        ProtocolType = ProtocolEnum.MsgSecret;
    }
    [ProtoMember(1)]
    public override ProtocolEnum ProtocolType { get; set; }
    [ProtoMember(2)]
    public string Secret;
}
[ProtoContract]
public class MsgPing : MsgBase
{
    public MsgPing()
    {
        ProtocolType = ProtocolEnum.MsgPing;
    }
    [ProtoMember(1)]
    public override ProtocolEnum ProtocolType { get; set; }
}
[ProtoContract]
public class MsgTest : MsgBase
{
    public MsgTest()
    {
        ProtocolType = ProtocolEnum.MsgTest;
    }
    [ProtoMember(1)]
    public override ProtocolEnum ProtocolType { get; set; }
    [ProtoMember(2)]
    public string ReqContent;
    [ProtoMember(3)]
    public string RecContent;
}
//注册协议
[ProtoContract]
public class MsgRegister : MsgBase
{
    public MsgRegister()
    {
        ProtocolType = ProtocolEnum.MsgRegister;
    }
    [ProtoMember(1)]
    public override ProtocolEnum ProtocolType { get; set; }
    [ProtoMember(2)]
    public string Account;
    [ProtoMember(3)]
    public string Password;
    [ProtoMember(4)]
    public RegisterType registerType;
    [ProtoMember(5)]
    public RegisterResult registerResult;
}
//登录协议
[ProtoContract]
public class MsgLogin : MsgBase
{
    public MsgLogin()
    {
        ProtocolType = ProtocolEnum.MsgLogin;
    }
    [ProtoMember(1)]
    public override ProtocolEnum ProtocolType { get; set; }
    [ProtoMember(2)]
    public string Account;
    [ProtoMember(3)]
    public string Password;
    [ProtoMember(4)]
    public LoginType loginType;
    [ProtoMember(5)]
    public LoginResult loginResult;
    [ProtoMember(6)]
    public string token;
}
//账号角色信息协议NEW
[ProtoContract]
public class MsgUserRole
{
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public string Username { get; set; }
    [ProtoMember(3)]
    public string RoleName { get; set; }
    [ProtoMember(4)]
    public int MaxLv { get; set; }
    [ProtoMember(5)]
    public int CurLv { get; set; }
    [ProtoMember(6)]
    public int CurEXP { get; set; }
    [ProtoMember(7)]
    public int CurHP { get; set; }
    [ProtoMember(8)]
    public int CurStrength { get; set; }
    [ProtoMember(9)]
    public int CurMP { get; set; }
    [ProtoMember(10)]
    public int CurATK { get; set; }
    [ProtoMember(11)]
    public int MaxGold { get; set; }
    [ProtoMember(12)]
    public int CurGold { get; set; }
    [ProtoMember(13)]
    public DateTime LastTime { get; set; }
    [ProtoMember(14)]
    public DateTime SignTime { get; set; }
    //任务数据，物品数据，技能数据
    [ProtoMember(15)]
    public List<CSFunData> FunItems { get; set; }
    [ProtoMember(16)]
    public List<CSBagData> BagItems { get; set; }
    [ProtoMember(17)]
    public List<CSTaskData> TaskItems { get; set; }
    [ProtoMember(18)]
    public int LastIndex { get; set; }
}
[ProtoContract]
public class MsgUpdateUserRole:MsgBase
{
    public MsgUpdateUserRole()
    {
        ProtocolType = ProtocolEnum.MsgUpdateUserRole;
    }
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public string Username { get; set; }
    [ProtoMember(3)]
    public string RoleName { get; set; }
    [ProtoMember(4)]
    public int MaxLv { get; set; }
    [ProtoMember(5)]
    public int CurLv { get; set; }
    [ProtoMember(6)]
    public int CurEXP { get; set; }
    [ProtoMember(7)]
    public int CurHP { get; set; }
    [ProtoMember(8)]
    public int CurStrength { get; set; }
    [ProtoMember(9)]
    public int CurMP { get; set; }
    [ProtoMember(10)]
    public int CurATK { get; set; }
    [ProtoMember(11)]
    public int MaxGold { get; set; }
    [ProtoMember(12)]
    public int CurGold { get; set; }
    [ProtoMember(13)]
    public DateTime LastTime { get; set; }
    [ProtoMember(14)]
    public DateTime SignTime { get; set; }
    [ProtoMember(15)]
    public int LastIndex { get; set; }
}

[ProtoContract]
public class CSFunData
{
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public int Type { get; set; }
    [ProtoMember(3)]
    public int Atk { get; set; }
    [ProtoMember(4)]
    public int NeedHp { get; set; }
    [ProtoMember(5)]
    public int NeedBlue { get; set; }
    [ProtoMember(6)]
    public int AddHp { get; set; }
    [ProtoMember(7)]
    public int AddBlue { get; set; }
    [ProtoMember(8)]
    public int OneLevel { get; set; }
    [ProtoMember(9)]
    public int NeedLv { get; set; }
    [ProtoMember(10)]
    public int IsStudy { get; set; }
    [ProtoMember(11)]
    public int CurLv { get; set; }
    [ProtoMember(12)]
    public int NeedGold { get; set; }
    [ProtoMember(13)]
    public bool IsEquip { get; set; }
    [ProtoMember(14)]
    public int ColdTime { get; set; }
}
[ProtoContract]
public class MsgStudyFun : MsgBase
{
    public MsgStudyFun()
    {
        ProtocolType = ProtocolEnum.MsgStudyFun;
    }
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public int Type { get; set; }
    [ProtoMember(3)]
    public int Atk { get; set; }
    [ProtoMember(4)]
    public int NeedHp { get; set; }
    [ProtoMember(5)]
    public int NeedBlue { get; set; }
    [ProtoMember(6)]
    public int AddHp { get; set; }
    [ProtoMember(7)]
    public int AddBlue { get; set; }
    [ProtoMember(8)]
    public int OneLevel { get; set; }
    [ProtoMember(9)]
    public int NeedLv { get; set; }
    [ProtoMember(10)]
    public int IsStudy { get; set; }
    [ProtoMember(11)]
    public int CurLv { get; set; }
    [ProtoMember(12)]
    public int NeedGold { get; set; }
    [ProtoMember(13)]
    public bool IsEquip { get; set; }
    [ProtoMember(14)]
    public int ColdTime { get; set; }
    [ProtoMember(15)]
    public string Username { get; set; }
    [ProtoMember(16)]
    public string RoleName { get; set; }
}
[ProtoContract]
public class MsgUpFun : MsgBase
{
    public MsgUpFun()
    {
        ProtocolType = ProtocolEnum.MsgUpFun;
    }
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public int Type { get; set; }
    [ProtoMember(3)]
    public int Atk { get; set; }
    [ProtoMember(4)]
    public int NeedHp { get; set; }
    [ProtoMember(5)]
    public int NeedBlue { get; set; }
    [ProtoMember(6)]
    public int AddHp { get; set; }
    [ProtoMember(7)]
    public int AddBlue { get; set; }
    [ProtoMember(8)]
    public int OneLevel { get; set; }
    [ProtoMember(9)]
    public int NeedLv { get; set; }
    [ProtoMember(10)]
    public int IsStudy { get; set; }
    [ProtoMember(11)]
    public int CurLv { get; set; }
    [ProtoMember(12)]
    public int NeedGold { get; set; }
    [ProtoMember(13)]
    public bool IsEquip { get; set; }
    [ProtoMember(14)]
    public int ColdTime { get; set; }
    [ProtoMember(15)]
    public string Username { get; set; }
    [ProtoMember(16)]
    public string RoleName { get; set; }
}
[ProtoContract]
public class CSBagData
{
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int StarNum;
    [ProtoMember(4)]
    public int CurLv;
    [ProtoMember(5)]
    public int IsEquip;
    [ProtoMember(6)]
    public int Type;
    [ProtoMember(7)]
    public int Atk;
    [ProtoMember(8)]
    public int Blue;
    [ProtoMember(9)]
    public int IsNew;
    [ProtoMember(10)]
    public int OneLv;

}
[ProtoContract]
public class MsgAddBagItem : MsgBase
{
    public MsgAddBagItem()
    {
        ProtocolType = ProtocolEnum.MsgAddBagItem;
    }
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int StarNum;
    [ProtoMember(4)]
    public int CurLv;
    [ProtoMember(5)]
    public int IsEquip;
    [ProtoMember(6)]
    public int Type { get; set; }
    [ProtoMember(7)]
    public int Atk { get; set; }
    [ProtoMember(8)]
    public int Blue { get; set; }
    [ProtoMember(9)]
    public int IsNew { get; set; }
    [ProtoMember(10)]
    public int OneLv { get; set; }
    [ProtoMember(11)]
    public string Username { get; set; }
    [ProtoMember(12)]
    public string RoleName { get; set; }

}
[ProtoContract]
public class MsgDeleteBagItem : MsgBase
{
    public MsgDeleteBagItem()
    {
        ProtocolType = ProtocolEnum.MsgDeleteBagItem;
    }
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public string Username { get; set; }
    [ProtoMember(4)]
    public string RoleName { get; set; }
}

[ProtoContract]
public class MsgUpdateBagItem : MsgBase
{
    public MsgUpdateBagItem()
    {
        ProtocolType = ProtocolEnum.MsgUpdateBagItem;
    }
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int StarNum;
    [ProtoMember(4)]
    public int CurLv;
    [ProtoMember(5)]
    public int IsEquip;
    [ProtoMember(6)]
    public int Type { get; set; }
    [ProtoMember(7)]
    public int Atk { get; set; }
    [ProtoMember(8)]
    public int Blue { get; set; }
    [ProtoMember(9)]
    public int IsNew { get; set; }
    [ProtoMember(10)]
    public int OneLv { get; set; }
    [ProtoMember(11)]
    public string Username { get; set; }
    [ProtoMember(12)]
    public string RoleName { get; set; }

}

[ProtoContract]
public class CSTaskData
{
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int CurNum;
    [ProtoMember(4)]
    public int MaxNum;
    [ProtoMember(5)]
    public int IsAccept;
    [ProtoMember(6)]
    public int IsCompelete;
    [ProtoMember(7)]
    public int IsCancel;
    [ProtoMember(8)]
    public int Exp;
    [ProtoMember(9)]
    public int Gold;

}
[ProtoContract]
public class MsgAddTask : MsgBase
{
    public MsgAddTask()
    {
        ProtocolType = ProtocolEnum.MsgAddTask;
    }
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int CurNum;
    [ProtoMember(4)]
    public int MaxNum;
    [ProtoMember(5)]
    public int IsAccept;
    [ProtoMember(6)]
    public int IsCompelete;
    [ProtoMember(7)]
    public int IsCancel;
    [ProtoMember(8)]
    public int Exp;
    [ProtoMember(9)]
    public int Gold;
    [ProtoMember(10)]
    public string Username { get; set; }
    [ProtoMember(11)]
    public string RoleName { get; set; }
}
[ProtoContract]
public class MsgUpdateTask : MsgBase
{
    public MsgUpdateTask()
    {
        ProtocolType = ProtocolEnum.MsgUpdateTask;
    }
    [ProtoMember(1)]
    public string Uid;
    [ProtoMember(2)]
    public int Id;
    [ProtoMember(3)]
    public int CurNum;
    [ProtoMember(4)]
    public int MaxNum;
    [ProtoMember(5)]
    public int IsAccept;
    [ProtoMember(6)]
    public int IsCompelete;
    [ProtoMember(7)]
    public int IsCancel;
    [ProtoMember(8)]
    public int Exp;
    [ProtoMember(9)]
    public int Gold;
    [ProtoMember(10)]
    public string Username { get; set; }
    [ProtoMember(11)]
    public string RoleName { get; set; }
}
[ProtoContract]
public class MsgDeleteTask : MsgBase
{
    public MsgDeleteTask()
    {
        ProtocolType = ProtocolEnum.MsgDeleteTask;
    }
    [ProtoMember(1)]
    public int Id;
    [ProtoMember(2)]
    public string Username { get; set; }
    [ProtoMember(3)]
    public string RoleName { get; set; }
}
[ProtoContract]
public class MsgUserRoles : MsgBase
{
    public MsgUserRoles()
    {
        ProtocolType = ProtocolEnum.MsgUserRoles;
    }
    [ProtoMember(1)]
    public string Username { get; set; }
    [ProtoMember(3)]
    public List<MsgUserRole> msgUserRoles;
}



