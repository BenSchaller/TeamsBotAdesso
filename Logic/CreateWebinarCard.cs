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

namespace EchoBot.Bots
{
    public class CreateWebinarCard
    {
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
            var card = AdaptiveCard.FromJson(File.ReadAllText("BusinessLogic\\Cards\\WebinarCard.json")).Card;
            

            var choicesFromDatabase = new DatabaseConnection();
            List<TerminData> terminList = new List<TerminData>();
            terminList = choicesFromDatabase.SqlConnection();
            var cs = new AdaptiveChoiceSetInput(); 
            cs.Id = Guid.NewGuid().ToString();
            cs.Value = "1";
           // AdaptiveChoiceSetInput choiceSet = JsonConvert.DeserializeObject<AdaptiveChoiceSetInput>(File.ReadAllText("BusinessLogic\\Cards\\ChoiceSet.json"));
            

            foreach (var choice in terminList)
            {
                AdaptiveChoice choices = JsonConvert.DeserializeObject<AdaptiveChoice>(RenderCardJsonFromDynamicJson(choice.Datum.ToString(), choice.ID.ToString()));
                cs.Choices.Add(choices);

            }
            card.Body.Add(cs);
            return card;
        }

        private string RenderCardJsonFromDynamicJson(string choiceTitle, string choiceValue)
        {
            var jsonBuilder = new StringBuilder(File.ReadAllText("BusinessLogic\\Cards\\Choice.json"));
            jsonBuilder.Replace("{choiceTitle}", choiceTitle);
            jsonBuilder.Replace("{choiceValue}", choiceValue);
            return jsonBuilder.ToString();
        }

        public string GetChoicesFromSql()
        {

            return null;
        }

        // SqlConnection sqlConnection { get; set; }
    }
}
