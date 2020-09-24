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
            SqlCommand selectCommand = new SqlCommand(selectString, connection);
            var result = selectCommand.ExecuteReader();
            
            if(result.FieldCount == 0)
            {
                string commandString = "Insert Into Webinarteilnehmer VALUES('" + userId + "', '" + userName + "')";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.ExecuteNonQuery();
            }

            sqlConnection.CloseSqlConnection(connection);
        }
    }
}
