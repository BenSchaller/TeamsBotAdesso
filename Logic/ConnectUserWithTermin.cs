using AdaptiveCards;
using EchoBot.DatabaseAccess;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Logic
{
    public class ConnectUserWithTermin
    {
        public void WriteInConnectionTable(TeamsChannelAccount user)
        {
            string userId = user.Id;
            var dbConnection = new DatabaseConnection();
            //dbConnection.InsertIntoConnectionTable(/*userId*/);
        }

    }
}
