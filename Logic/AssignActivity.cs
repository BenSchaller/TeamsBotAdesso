using EchoBot.Bots;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.Logic
{
    public class AssignActivity
    {
        private ITurnContext<IMessageActivity> turnContext;
        private LuisRecognizer luisRecognizer;
        private CancellationToken cancellationToken;
        private QnAMaker echoBotQnA;
        public AssignActivity(ITurnContext<IMessageActivity> context, CancellationToken token, LuisRecognizer recognizer, QnAMaker qna)
        {
            turnContext = context;
            luisRecognizer = recognizer;
            cancellationToken = token;
            echoBotQnA = qna;
        }


        public async Task Assigner()
        {
            var activity = turnContext.Activity;
            UseUserInformation userInformation = new UseUserInformation();

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
                    await turnContext.SendActivityAsync("Bitte erst den Buchungsvorgang Abschließen");
                    if (activity.Text != null && activity.Value == null)
                    {
                        //Access the Luis class
                        var luisRouting = new LuisAccess(luisRecognizer);

                        //Use IdentifiedIntent class to check if Question

                        if (await luisRouting.GetIntent(turnContext, cancellationToken) == IdentifiedIntent.None)
                        {
                            await QnA();
                        }
                        else
                        {
                            await Webinar(userInformation);
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



            }

        }
        public async Task Webinar(UseUserInformation userInformation)
        {
            var webinarCard = new CreateWebinarCard(turnContext);
            var card = webinarCard.GetWebinarCardFromJson();

            await turnContext.SendActivityAsync(MessageFactory.Attachment(card));


            var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            userInformation.CreateNewUserEntry(member);

        }
        public async Task QnA()
        {
            var qnaMaker = new QnAMakerAccess(echoBotQnA);
            await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);

        }

    }
}
