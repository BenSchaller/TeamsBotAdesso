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
using Microsoft.Bot.Builder;
using System.Threading;
using EchoBot.ConversationStateHandler;
using System.Linq;

namespace EchoBot.Bots
{
    public class CreateWebinarCard
    {
        //private ConversationState _conversationState;
        ITurnContext turnContext;
        private AdaptiveCard card;

        public CreateWebinarCard(ITurnContext context)
        {
            turnContext = context;
        }

        public Attachment GetWebinarCardFromJson()
        {
            Attachment card = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = BuildAdaptiveCard()
            };

            return card;
        }

        private AdaptiveCard BuildAdaptiveCard()
        {
            card = AdaptiveCard.FromJson(File.ReadAllText("BusinessLogic\\Cards\\WebinarCard.json")).Card;

            var webinarTermine = new GetWebinarTermine();
            List<TerminData> terminList = new List<TerminData>();
            terminList = webinarTermine.GetTermineFromSql();
            var choiceSet = new AdaptiveChoiceSetInput();
            choiceSet.Id = Guid.NewGuid().ToString();
            choiceSet.Value = "1";
            choiceSet.Type = "Input.ChoiceSet";

            foreach (var choice in terminList)
            {
                AdaptiveChoice choices = JsonConvert.DeserializeObject<AdaptiveChoice>(RenderCardJsonFromDynamicJson(choice.Datum.ToString(), choice.ID.ToString()));

                choiceSet.Choices.Add(choices);
            }
            card.Body.Add(choiceSet);

            return card;
        }

        public Activity CreateReply(AdaptiveCard card)
        {
            var reply = turnContext.Activity.CreateReply();
            reply.Attachments = new List<Attachment>()
            {
                new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = card
                }
            };
            return reply;
        }

        private string RenderCardJsonFromDynamicJson(string choiceTitle, string choiceValue)
        {
            var jsonBuilder = new StringBuilder(File.ReadAllText("BusinessLogic\\Cards\\Choice.json"));
            jsonBuilder.Replace("{choiceTitle}", choiceTitle);
            jsonBuilder.Replace("{choiceValue}", choiceValue);
            return jsonBuilder.ToString();
        }
    }
}
