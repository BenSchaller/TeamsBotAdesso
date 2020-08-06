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
               
                var attachment = MessageFactory.Attachment(GetHeroCardFromQnaResult(resultFirst.Answer).ToAttachment());
                await turnContext.SendActivityAsync(attachment, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker hat keine Antwort gefunden."), cancellationToken);
            }
        }

        private static HeroCard GetHeroCardFromQnaResult(string answer)
        {
            var heroCard = new HeroCard();

            //XmlTextReader heroCardDocument = new XmlTextReader(answer);
            //Xml Format:
            //< Karte name = "heroCard" >
            //  < title > Chat starten </ title >
            //  < text > Webside </text >
            //  < URL > https://support.microsoft.com/de-de/office/starten-und-anheften-von-chats-a864b052-5e4b-4ccf-b046-2e26f40e21b5 </URL>
            //</ Karte >
            XmlDocument heroCardDocument = new XmlDocument();
            heroCardDocument.Load(answer);
            string[] heroCardContent = new string[2];
            int arrayCounter = 0;

            foreach (XmlNode node in heroCardDocument.ChildNodes)
            {
                heroCardContent[arrayCounter] = node.Attributes[arrayCounter].InnerText;
                arrayCounter++;
            }

            var title = heroCardContent[0];
            var textBtn = heroCardContent[1];
            var url = heroCardContent[2];



            //var titleExpr = new Regex(@"/### ([^\\]+)/", RegexOptions.Compiled);
            //var title = titleExpr.Match(answer).Value;

            heroCard.Title = title;
            heroCard.Text = answer;
            heroCard.Buttons = new List<CardAction>()
            {
                new CardAction() { Value = url, Title = textBtn, Type = ActionTypes.OpenUrl }
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
