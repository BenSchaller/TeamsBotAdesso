using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.DatabaseAccess
{
    public class DatabaseConnection
    {
        string sqlConnectionString = "Server=tcp:azuredbforwebinar.database.windows.net,1433;Initial Catalog=WebinarDB;Persist Security Info=False;" +
            "User ID=benschaller;Password=Admin1234!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        SqlConnection sql = new SqlConnection();
    }
}
