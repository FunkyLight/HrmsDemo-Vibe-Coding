using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace HrmsDemo.Helpers
{
    public static class DbHelper
    {
        // 預設連線字串，使用者應根據實際環境修改
        // Default Key: HrmsDb
        public static string ConnectionString { get; set; } = "Server=localhost;Database=HrmsDB;Uid=root;Pwd=123456789;";

        public static IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
