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
            //var userInformation = new GetUserInformation();
            //var userInformationList = userInformation.UserInformation();

            

            string userId = member.Email;
            string userName = member.Name;

            UpdateSqlDb(userId, userName);
        }

        public void UpdateSqlDb(string userId, string userName)
        {
            var sqlConnection = new DatabaseConnection();
            var connection = sqlConnection.OpenSqlConnection();

            string commandString = "Insert Into Webinarteilnehmer VALUES('" + userId + "', '" + userName + "')";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();

            sqlConnection.CloseSqlConnection(connection);

        }
    }
}
