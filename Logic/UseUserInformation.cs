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

            DatabaseConnection databaseConnection = new DatabaseConnection();

            databaseConnection.InsertTeilnehmerInDb(userMail, userName);
        }        
    }
}
