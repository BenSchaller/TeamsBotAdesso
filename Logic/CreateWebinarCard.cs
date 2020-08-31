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

            //kommentar
            return card;
        }

        private AdaptiveCard BuildAdaptiveCard()
        {
            var card = AdaptiveCard.FromJson(File.ReadAllText("BusinessLogic\\Cards\\WebinarCard.json")).Card;

            //var choicesFromDatabase = new DatabaseConnection();
            //List<TerminData> terminList = new List<TerminData>();
            //terminList = choicesFromDatabase.SqlConnection();
            var choiceSet = new AdaptiveChoiceSetInput();
            choiceSet = JsonConvert.DeserializeObject<AdaptiveChoiceSetInput>(File.ReadAllText("BusinessLogic\\Cards\\ChoiceSet.json"));


            //foreach (var choice in terminList)
            //{
                AdaptiveChoice choices = JsonConvert.DeserializeObject<AdaptiveChoice>(RenderCardJsonFromDynamicJson("24.02.2020", "1"/*choice.Datum.ToString(), choice.ID.ToString()*/));
                choiceSet.Choices.Add(choices);

            //}
            card.Body.Add(choiceSet);
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
