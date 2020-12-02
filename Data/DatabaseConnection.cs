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

        public bool DidUserBookWebinar(string terminId, string userId)
        {

            sqlConnection.Open();

            string checkUserTerminConnection = "SELECT * FROM Termine2Teilnehmer where TerminId = @terminId AND TeilnehmerId = @userId";

            SqlCommand checkIfUserIsBookedCommand = new SqlCommand(checkUserTerminConnection, sqlConnection);
            checkIfUserIsBookedCommand.Parameters.AddWithValue("@terminId", terminId);
            checkIfUserIsBookedCommand.Parameters.AddWithValue("@userId", userId);
            var result = checkIfUserIsBookedCommand.ExecuteReader();
            bool userIsInTable = result.HasRows;

            sqlConnection.Close();
            return userIsInTable;

        }

        public void InsertIntoConnectionTable(string terminId, string userId)
        {
            sqlConnection.Open();
            string commandString = "INSERT INTO Termine2Teilnehmer VALUES(@terminId, @userId)";
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
            string selectId = "Select * from WebinarTeilnehmer where Convert(varchar(60), MailAdresse) = @userMail";
            SqlCommand selectUserIdCommand = new SqlCommand(selectId, sqlConnection);
            selectUserIdCommand.Parameters.AddWithValue("@userMail", userMail);

            var result = selectUserIdCommand.ExecuteReader();
            bool hasRows = result.HasRows;
            sqlConnection.Close();

            return hasRows;
        }

        public List<TerminData> GetWebinarTermine()
        {
            sqlConnection.Open();
            string terminAbfrageString = "Select ID, Datum, StartZeit, EndZeit from dbo.WebinarTermine";

            SqlCommand command = new SqlCommand(terminAbfrageString, sqlConnection);
            using (SqlDataReader terminReader = command.ExecuteReader())
            {
                StringBuilder builder = new StringBuilder();
                var terminList = new List<TerminData>();
                while (terminReader.Read())
                {
                    terminList.Add(new TerminData { ID = terminReader.GetInt32(0), Datum = terminReader.GetDateTime(1), Startzeit = terminReader.GetTimeSpan(2), Endzeit = terminReader.GetTimeSpan(3) });
                }
                terminReader.Close();
                sqlConnection.Close();
                return terminList;
            }
        }
        public void InsertTeilnehmerInDb(string userMail, string userName)
        {
            sqlConnection.Open();

            string selectString = "Select MailAdresse from Webinarteilnehmer where CONVERT(VARCHAR(60), MailAdresse) = @userMail";
            SqlCommand selectUserByMailCmd = new SqlCommand(selectString, sqlConnection);
            selectUserByMailCmd.Parameters.AddWithValue("@userMail", userMail);
            bool userIsInDb = selectUserByMailCmd.ExecuteReader().HasRows;
            sqlConnection.Close();

            sqlConnection.Open();
            if (!userIsInDb)
            {
                string insertString = "Insert Into Webinarteilnehmer VALUES(@userName, @userMail)";
                SqlCommand insertUserCmd = new SqlCommand(insertString, sqlConnection);
                insertUserCmd.Parameters.AddWithValue("@userName", userName);
                insertUserCmd.Parameters.AddWithValue("@userMail", userMail);
                insertUserCmd.ExecuteNonQuery();
            }

            sqlConnection.Close();
        }
    }
}