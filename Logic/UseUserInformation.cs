using EchoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EchoBot.DatabaseAccess;
using System.Data.SqlClient;
using Microsoft.Bot.Schema.Teams;

namespace EchoBot.Logic
{
    public class UseUserInformation
    {
        public void CreateNewUserEntry(TeamsChannelAccount member)
        {
            string userId = member.Email;
            string userName = member.Name;

            UpdateSqlDb(userId, userName);
        }

        public void UpdateSqlDb(string userId, string userName)
        {
            var sqlConnection = new DatabaseConnection();
            var connection = sqlConnection.OpenSqlConnection();
            
            string selectString = "Select * from Webinarteilnehmer where MailAdresse = " + userId;
            SqlCommand selectUserByMailCmd = new SqlCommand(selectString, connection);
            SqlDataReader result = selectUserByMailCmd.ExecuteReader();
            
            //if(result.HasRows)
            //{
                string commandString = "Insert Into Webinarteilnehmer VALUES('" + userId + "', '" + userName + "')";
                SqlCommand insertUserCmd = new SqlCommand(commandString, connection);

                insertUserCmd.ExecuteNonQuery();
            //}

            sqlConnection.CloseSqlConnection(connection);
        }
    }
}
