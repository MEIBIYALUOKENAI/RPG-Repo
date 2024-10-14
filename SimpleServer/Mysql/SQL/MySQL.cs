using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mysql.SQL
{
    public class MySQL : Singleton<MySQL>
    {
        //数据库
        public SqlSugarClient sqlSugarDB;
        public void Init()
        {
            //配置数据库，与mysql数据库连接
            sqlSugarDB = new SqlSugarClient(
    new ConnectionConfig()
    {
        ConnectionString = "server=localhost;uid=sa;pwd=1234;database=test1",
        DbType = DbType.SqlServer,//设置数据库类型
        IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
        InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
    });
            Debug.LogInfo("数据库链接成功！");
        }
    }
}
