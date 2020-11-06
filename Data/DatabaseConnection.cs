using EchoBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoBot.DatabaseAccess
{
    public class DatabaseConnection
    {
        private SqlConnection sqlConnection;
        public DatabaseConnection()
        {
            string sqlConnectionString = "Server=tcp:webinarazuresqldb.database.windows.net,1433;Initial Catalog=WebinarDB;Persist Security Info=False;" +
            "User ID=benschaller;Password=Admin1234!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            sqlConnection = new SqlConnection(sqlConnectionString);
        }
        public SqlConnection OpenSqlConnection()
        {
            sqlConnection.Open();
            return sqlConnection;
        }

        public void CloseSqlConnection()
        {
            sqlConnection.Close();
        }

        public void InsertIntoConnectionTable(int terminId/*string userMail*/)
        {
            string userMail = "Benedict-Vincent.Schaller@adesso.de";
            sqlConnection.Open();
            string selectId = "Select Id from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = @userMail";
            SqlCommand selectIdSql = new SqlCommand(selectId, sqlConnection);

            selectIdSql.Parameters.AddWithValue("@userMail", userMail);

            int userId = (int)selectIdSql.ExecuteScalar();
            string commandString = "INSERT INTO Termine2Teilnehmer VALUES('" + terminId + "', '" + userId + "')";
            SqlCommand command = new SqlCommand(commandString, sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
