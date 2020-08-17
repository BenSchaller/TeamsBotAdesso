// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.Luis;
using EchoBot.Bots;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Returns the raw Context
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            //Access the QnAMaker class
            QnAMakerAccess qnaMaker = new QnAMakerAccess(EchoBotQnA);
            await qnaMaker.GetRequest(turnContext, cancellationToken);

            //Access the Luis class
            LuisAccess luisRouting = new LuisAccess(LuisNavigation);
            await luisRouting.GetRequest(turnContext, cancellationToken);
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

        //Construct connection to Microsoft Cognitive Services
        public LuisRecognizer LuisNavigation { get; private set; }

        public QnAMaker EchoBotQnA { get; private set; }


        public TeamsBot (LuisRecognizerOptionsV3 optionsLuis, QnAMakerEndpoint endpoint)
        {
            //Connects to QnAMakerEnpoint for each turn
            LuisNavigation = new LuisRecognizer(optionsLuis);
            EchoBotQnA = new QnAMaker(endpoint);
        }
    }
}
