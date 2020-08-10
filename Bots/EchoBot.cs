// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.AI.QnA;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NuGet.RuntimeModel;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            //    var replyTime = $"Die aktuelle Uhrzeit ist {DateTime.Now.AddHours(2)}";
            //    await turnContext.SendActivityAsync(MessageFactory.Text(replyTime, replyTime), cancellationToken);

            //Replies from the Bot - Echo and QnAMaker
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            await AccessQnAMaker(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            //Willkommensnachricht beim starten des Bots
            var welcomeText = "Herzlich Willkommen!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        private async Task AccessQnAMaker (ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Send it to the QnAMaker
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);

            //Result from QnAMaker?
            if (results.Any())
            {
                var resultFirst = results.First();

                var attachment = MessageFactory.Attachment(FillHeroCardArray(resultFirst.Answer).ToAttachment());
                await turnContext.SendActivityAsync(attachment, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker hat keine Antwort gefunden."), cancellationToken);
            }
        }

        public static HeroCard FillHeroCardArray(string answer)
        {
            //Read and Parse JSONString to JSONObject
            
            //var getObjects = JObject.Parse(jsonFile);

            //JsonConvert.DeserializeObject(answer);
            var getObjects = JObject.Parse(answer);

            //Write JSONObject in String
            string titleJson, buttonDescriptionJson, urlJson, imageUrlJson;
            titleJson = (string)getObjects["title"];
            buttonDescriptionJson = (string)getObjects["buttonDesc"];
            urlJson = (string)getObjects["url"];
            imageUrlJson = (string)getObjects["imgUrl"];




            HeroCard heroCard = CreateHeroCardFromArray(titleJson, buttonDescriptionJson, urlJson, imageUrlJson);

            return heroCard;
        }

        public static HeroCard CreateHeroCardFromArray(string title, string buttonDescription, string url, string imageUrl)
        {
            HeroCard heroCard = new HeroCard
            {
                Title = title,
            };

            heroCard.Buttons = new List<CardAction>
            {
                new CardAction() { Value = url, Title = buttonDescription, Type = ActionTypes.OpenUrl }
            };

            heroCard.Images = new List<CardImage>
            {
                new CardImage( url = imageUrl)
            };

            return heroCard;
        }

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot (QnAMakerEndpoint endpoint)
        {
            //Connects to QnAMakerEnpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }

    }
}
