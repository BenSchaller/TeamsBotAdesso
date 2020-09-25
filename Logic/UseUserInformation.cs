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

            string selectString = "Select MailAdresse from Webinarteilnehmer where MailAdresse = '" + userId + "'";
            SqlCommand selectUserByMailCmd = new SqlCommand(selectString, connection);
            var result = selectUserByMailCmd.ExecuteReader();

            if (result.HasRows)
            {
                string insertString = "Insert Into Webinarteilnehmer VALUES('" + userName + "', '" + userId + "')";
                SqlCommand insertUserCmd = new SqlCommand(insertString, connection);

                insertUserCmd.ExecuteNonQuery();
            }


            sqlConnection.CloseSqlConnection(connection);
        }
    }
}
