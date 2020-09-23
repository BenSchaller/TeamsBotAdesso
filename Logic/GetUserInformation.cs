using Microsoft.Bot.Schema;
using AdaptiveCards;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using System.Text;
using System;
using TeamsBot.DatabaseAccess;
using TeamsBot.Data;
using System.Collections.Generic;

namespace TeamsBot.Data
{
    public class GetUserInformation
    {
        public MicrosoftTeamUser UserInformation()
        {
            List<MicrosoftTeamUser> teamUsers = new List<MicrosoftTeamUser>();
            string email = string.Empty;
            ConversationMembers conversationMembers = new ConversationMembers();
            ConversationAccount conversationAccount = new ConversationAccount();
            
            teamUsers.Add(JsonConvert.DeserializeObject<MicrosoftTeamUser>(conversationAccount.Id));
            
            MicrosoftTeamUser user = new MicrosoftTeamUser{
            ID = conversationAccount.Id,
            Name = conversationAccount.Name,
            };
            return user;
        }
    }

}
