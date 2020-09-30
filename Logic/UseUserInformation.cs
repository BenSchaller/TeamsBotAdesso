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
            string userMail = member.Email;
            string userName = member.Name;

            UpdateSqlDb(userMail, userName);
        }

        public void UpdateSqlDb(string userMail, string userName)
        {
            var sqlConnection = new DatabaseConnection();
            var connection = sqlConnection.OpenSqlConnection();

            //string selectString = "Select MailAdresse from Webinarteilnehmer where CONVERT(VARCHAR, Name) = '" + userName + "'";
            //SqlCommand selectUserByMailCmd = new SqlCommand(selectString, connection);
            //var result = selectUserByMailCmd.ExecuteReader();

            //if (result.HasRows == false)
            //{
                string insertString = "Insert Into Webinarteilnehmer VALUES('" + userName + "', '" + userMail + "')";
                SqlCommand insertUserCmd = new SqlCommand(insertString, connection);

                insertUserCmd.ExecuteNonQuery();
            //}


            sqlConnection.CloseSqlConnection();
        }
    }
}
