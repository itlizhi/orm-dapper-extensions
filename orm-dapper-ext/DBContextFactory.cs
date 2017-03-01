using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Orm
{
    public class DBContextFactory
    {
        /// <summary>
        ///  DBContext工厂方法
        /// </summary>
        /// <param name="type">数据库类型，目前支持sqlserver，mysql </param>
        /// <param name="conn">连接字符串 conn</param>
        /// <returns></returns>
        public static IDBContext CreateContext(string type, string conn = "defconn")
        {
            type = type.ToLower();
            switch (type)
            {
                case "sqlserver":
                    return new SqlServerDBContext(conn);
                case "mysql":
                    return new MySqlDBContext(conn);
                default:
                    throw new Exception("没用找的IDBContext的实例类型");
            }
        }
    }
}
