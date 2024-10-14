using SimpleServer.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mysql.SQL;

namespace SimpleServer
{
    //程序的入口
    class Program
    {
        static void Main(string[] args)
        {
            //数据库连接，初始化
            MySQL.Instance.Init();

            ServerSocket.Instance.Init();

        }
    }
}
