using SqlSugar;
using System;

namespace Mysql.SQL
{
    [SugarTable("User")]
    public class User
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DateTime Logindate { get; set; }

        public LoginType Logintype { get; set; }
        public string Token { get; set; }
    }
   
    [SugarTable("UserRole")]
    public class UserRole
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public int MaxLv { get; set; }
        public int CurLv { get; set; }
        public int CurEXP { get; set; }
        public int CurHP { get; set; }
        public int CurStrength { get; set; }
        public int CurMP { get; set; }
        public int CurATK { get; set; }
        public int MaxGold { get; set; }
        public int CurGold { get; set; }
        public DateTime LastTime { get; set; }
        public DateTime SignTime { get; set; }
        public int LastIndex { get; set; }
    }

    [SugarTable("TaskData")]
    public class TaskData
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idd { get; set; }
        public string Uid { get; set; }
        public int Id { get; set; }
        public int CurNum { get; set; }
        public int MaxNum { get; set; }
        public int IsAccept { get; set; }
        public int IsCompelete { get; set; }
        public int IsCancel { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }

    }

    [SugarTable("FunData")]
    public class FunData
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idd { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public int Atk { get; set; }
        public int NeedHp { get; set; }
        public int NeedBlue { get; set; }
        public int AddHp { get; set; }
        public int AddBlue { get; set; }
        public int OneLevel { get; set; }
        public int NeedLv { get; set; }
        public int IsStudy { get; set; }
        public int CurLv { get; set; }
        public int NeedGold { get; set; }
        public bool IsEquip { get; set; }
        public int ColdTime { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }   
    }

    [SugarTable("BagData")]
    public class BagData
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idd { get; set; }

        public string Username { get; set; }
        public string RoleName { get; set; }
        public string Uid { get; set; }
        public int Id { get; set; }
        public int StarNum { get; set; }
        public int CurLv { get; set; }
        public int IsEquip { get; set; }
        public int Type { get; set; }
        public int Atk { get; set; }
        public int Blue { get; set; }
        public int IsNew { get; set; }
        public int OneLv { get; set; }
    }
}
