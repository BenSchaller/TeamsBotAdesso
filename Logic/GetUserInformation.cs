using Microsoft.Bot.Schema;
using AdaptiveCards;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using System.Text;
using System;
using EchoBot.DatabaseAccess;
using EchoBot.Data;
using System.Collections.Generic;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Builder.Teams;

namespace EchoBot.Data
{
    public class GetUserInformation
    {
        public MicrosoftTeamUser UserInformation()
        {
            ConversationAccount conversationAccount = new ConversationAccount();
            ConversationMembers conversationMembers = new ConversationMembers();
            
            ChannelAccount channelAccount = new ChannelAccount();
            
            
            MicrosoftTeamUser user = new MicrosoftTeamUser{
            ID = channelAccount.Id,
            Name = channelAccount.Name,
            };

            return user;
        }
    }

}
