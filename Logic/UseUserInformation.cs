using EchoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EchoBot.DatabaseAccess;
using System.Data.SqlClient;

namespace EchoBot.Logic
{
    public class UseUserInformation
    {
        public void CreateNewUserEntry()
        {
            var userInformation = new GetUserInformation();
            var userInformationList = userInformation.UserInformation();

            string userId = userInformationList.ID;
            string userName = userInformationList.Name;

            UpdateSqlDb(userId, userName);
        }

        public void UpdateSqlDb(string userId, string userName)
        {
            var sqlConnection = new DatabaseConnection();
            var connection = sqlConnection.OpenSqlConnection();

            string commandString = "Insert Into Webinarteilnehmer VALUES(" + userId + ", " + userName + ")";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();

            sqlConnection.CloseSqlConnection(connection);

        }
    }
}
