// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.Luis;
using EchoBot.Bots;
using EchoBot.Logic;
using Microsoft.Bot.Builder.Teams;
using Newtonsoft.Json.Linq;
using AdaptiveCards;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            ////Returns the raw Context/Echo Kommentar

            //UseUserInformation userInformation = new UseUserInformation();

            //var activity = turnContext.Activity;
            //if(string.IsNullOrEmpty(activity.Text) && activity.Value!= null)
            //{
            //    await turnContext.SendActivityAsync(MessageFactory.Text("Schreib das hier"), cancellationToken);
            //}

            ////await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            ////Access the Luis class
            //var luisRouting = new LuisAccess(LuisNavigation);

            ////Use IdentifiedIntent class to check if Question

            //if (await luisRouting.GetIntent(turnContext, cancellationToken) == IdentifiedIntent.None)
            //{
            //    var qnaMaker = new QnAMakerAccess(EchoBotQnA);
            //    await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);
            //}
            //else
            //{
            //    var webinarCard = new CreateWebinarCard();
            //    var card = webinarCard.GetWebinarCardFromJson();

            //    await turnContext.SendActivityAsync(MessageFactory.Attachment(card));


            //    var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            //    userInformation.CreateNewUserEntry(member);
            //}
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            //Returns the raw Context/Echo Kommentar

            UseUserInformation userInformation = new UseUserInformation();

            var activity = turnContext.Activity;
            switch (turnContext.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    var welcomeText = "Herzlich Willkommen!";
                    IList<ChannelAccount> membersAdded = new List<ChannelAccount>();
                    foreach (var member in membersAdded)
                    {
                        if (member.Id != turnContext.Activity.Recipient.Id)
                        {
                            await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                        }
                    }
                    break;

                case ActivityTypes.Message:

                    if (activity.Text != null && activity.Value == null)
                    {
                        //Access the Luis class
                        var luisRouting = new LuisAccess(LuisNavigation);

                        //Use IdentifiedIntent class to check if Question

                        if (await luisRouting.GetIntent(turnContext, cancellationToken) == IdentifiedIntent.None)
                        {
                            var qnaMaker = new QnAMakerAccess(EchoBotQnA);
                            await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);
                        }
                        else
                        {
                            var webinarCard = new CreateWebinarCard(turnContext);
                            var card = webinarCard.GetWebinarCardFromJson();

                            await turnContext.SendActivityAsync(MessageFactory.Attachment(card));


                            var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
                            userInformation.CreateNewUserEntry(member);
                        }

                        break;

                    }
                    else if (string.IsNullOrEmpty(activity.Text) && activity.Value != null)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("Ich bin nun im Buchungszweig"), cancellationToken);
                        var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

                        ConnectUserWithTermin connectUser = new ConnectUserWithTermin();
                        connectUser.WriteInConnectionTable(member);

                        break;
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("Es gab einen Fehler..."), cancellationToken);

                        break;
                    }

            };
        }

        //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);




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


        public TeamsBot(LuisRecognizerOptionsV3 optionsLuis, QnAMakerEndpoint endpoint)
        {
            //Connects to QnAMakerEnpoint for each turn
            LuisNavigation = new LuisRecognizer(optionsLuis);
            EchoBotQnA = new QnAMaker(endpoint);
        }
    }
}
