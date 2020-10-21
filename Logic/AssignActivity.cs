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
        private CancellationToken cancellationToken;
        private QnAMaker echoBotQnA;
        private LuisRecognizer luisRecognizer;

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

            if (activity.Text != null && activity.Value == null)
            {
                await turnContext.SendActivityAsync("Bitte erst den Buchungsvorgang abschließen oder abbrechen");
            }

            else if (string.IsNullOrEmpty(activity.Text) && activity.Value != null)
            {
                
                await turnContext.SendActivityAsync(MessageFactory.Text("Es wurde ein Knopf gedrückt"), cancellationToken);
                var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

                ConnectUserWithTermin connectUser = new ConnectUserWithTermin();
                connectUser.WriteInConnectionTable(member);

            }

            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Es gab einen Fehler..."), cancellationToken);

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

