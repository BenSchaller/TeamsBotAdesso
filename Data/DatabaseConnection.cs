using TeamsBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsBot.DatabaseAccess
{
    public class DatabaseConnection
    {
        public SqlConnection OpenSqlConnection()
        {
            string sqlConnectionString = "Server=tcp:webinarazuresqldb.database.windows.net,1433;Initial Catalog=WebinarDB;Persist Security Info=False;" +
             "User ID=benschaller;Password=Admin1234!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

            sqlConnection.Open();
            return sqlConnection;
        }

        public void CloseSqlConnection(SqlConnection sqlConnection)
        {
            sqlConnection.Close();
        }

    }
}
