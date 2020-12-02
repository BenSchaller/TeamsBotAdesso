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
            
            string selectId = "Select Id from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = @userMail";

            SqlCommand selectUserIdCommand = new SqlCommand(selectId, sqlConnection);
            selectUserIdCommand.Parameters.AddWithValue("@userMail", userMail);

            string userId = selectUserIdCommand.ExecuteScalar().ToString();

            sqlConnection.Close();
            return userId;
        }

        public bool DidUserBookWebinar(int terminId, string userId)
        {

            sqlConnection.Open();

            string checkUserTerminConnection = "SELECT * FROM Termine2Teilnehmer where TerminId = '@terminId', TeilnehmerId = '@userId'";
            
            SqlCommand checkIfUserIsBookedCommand = new SqlCommand(checkUserTerminConnection, sqlConnection);
            checkIfUserIsBookedCommand.Parameters.AddWithValue("@terminId", terminId);
            checkIfUserIsBookedCommand.Parameters.AddWithValue("@userId", userId);
            var result = checkIfUserIsBookedCommand.ExecuteReader();

            sqlConnection.Close();
            return result.HasRows;

        }

        public void InsertIntoConnectionTable(int terminId, string userId)
        {
            sqlConnection.Open();
            string commandString = "INSERT INTO Termine2Teilnehmer VALUES('@terminId', '@userId')";
            SqlCommand insertInConnectionTableCommand = new SqlCommand(commandString, sqlConnection);
            insertInConnectionTableCommand.Parameters.AddWithValue("@terminId", terminId);
            insertInConnectionTableCommand.Parameters.AddWithValue("@userId", userId);

            insertInConnectionTableCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public bool IsUserInDatabase(string userMail)
        {
            sqlConnection.Open();
            //userMail = "Benedict-Vincent.Schaller@adesso.de";
            string selectId = "Select * from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = '@userMail'";
            SqlCommand selectUserIdCommand = new SqlCommand(selectId, sqlConnection);
            selectUserIdCommand.Parameters.AddWithValue("@userMail", userMail);

            var result = selectUserIdCommand.ExecuteReader();
            bool hasRows = result.HasRows;
            sqlConnection.Close();

            return hasRows;
        }

    }
}