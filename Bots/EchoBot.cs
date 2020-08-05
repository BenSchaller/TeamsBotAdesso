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

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //if (turnContext.Activity.Text == "Uhrzeit")
            //{
            //    var replyTime = $"Die aktuelle Uhrzeit ist {DateTime.Now.AddHours(2)}";
            //    await turnContext.SendActivityAsync(MessageFactory.Text(replyTime, replyTime), cancellationToken);
            //}
            //else
            //{
                var replyText = $"Echo: {turnContext.Activity.Text}";
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            //}

            await AccessQnAMaker(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
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
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Das Ergebnis des QnA Maker ergab: {0}", results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker hat keine passende Antwort gefunden."), cancellationToken);
            }
        }

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot (QnAMakerEndpoint endpoint)
        {
            //Connects to QnAMakerEnpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }

    }
}
