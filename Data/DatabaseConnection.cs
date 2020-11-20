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

        public string GetUserIdByMail(string userMail)
        {
            sqlConnection.Open();
            string selectId = "Select Id from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = '@userMail'";
            SqlCommand selectIdSql = new SqlCommand(selectId, sqlConnection);
            selectIdSql.Parameters.AddWithValue("@userMail", userMail);

            string userId = selectIdSql.ExecuteScalar().ToString();

            sqlConnection.Close();
            return userId;
        }

        public bool DidUserBookWebinar(int terminId, string userId)
        {
            sqlConnection.Open();

            string checkUserTerminConnection = "SELECT * FROM Termine2Teilnehmer where TerminId = @terminId, TeilnehmerId = @userId";
            SqlCommand checkIfUserIsBooked = new SqlCommand(checkUserTerminConnection, sqlConnection);
            checkIfUserIsBooked.Parameters.AddWithValue("@terminId", terminId);
            checkIfUserIsBooked.Parameters.AddWithValue("@userId", userId);
            var result = checkIfUserIsBooked.ExecuteReader();

            sqlConnection.Close();
            return result.HasRows;

        }

        public void InsertIntoConnectionTable(int terminId, string userId)
        {
            sqlConnection.Open();
            string commandString = "INSERT INTO Termine2Teilnehmer VALUES(@terminId, @userId)";
            SqlCommand command = new SqlCommand(commandString, sqlConnection);
            command.Parameters.AddWithValue("@terminId", terminId);
            command.Parameters.AddWithValue("@userId", userId);

            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public bool CheckUserEntry(string userMail)
        {
            sqlConnection.Open();
            //userMail = "Benedict-Vincent.Schaller@adesso.de";
            string selectId = "Select * from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = '@userMail'";
            SqlCommand selectIdSql = new SqlCommand(selectId, sqlConnection);
            selectIdSql.Parameters.AddWithValue("@userMail", userMail);

            var result = selectIdSql.ExecuteReader();
            bool hasRows = result.HasRows;
            sqlConnection.Close();

            return hasRows;
        }

    }
}