using AdaptiveCards;
using EchoBot.Bots;
using EchoBot.ConversationStateHandler;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using EchoBot.DatabaseAccess;

namespace EchoBot.Logic
{
    public class AssignActivity
    {
        private ITurnContext<IMessageActivity> turnContext;
        private CancellationToken cancellationToken;
        private QnAMaker echoBotQnA;
        public AssignActivity(ITurnContext<IMessageActivity> context, CancellationToken token, QnAMaker qna)
        {
            turnContext = context;
            cancellationToken = token;
            echoBotQnA = qna;
        }


        public async Task<bool> Assigner()
        {
            var activity = turnContext.Activity;
            UseUserInformation userInformation = new UseUserInformation();

            if (activity.Text != null && activity.Value == null)
            {
                await turnContext.SendActivityAsync("Bitte erst den Buchungsvorgang abschließen oder abbrechen");
                return true;
            }

            else if (string.IsNullOrEmpty(activity.Text) && activity.Value != null)
            {
                string value = turnContext.Activity.Value.ToString();

                if (value.Contains("book"))
                {
                    string[] items = new string[10];
                    items = value.Split('"');
                    string terminId = items[7];
                    int termId = Int32.Parse(terminId);
                    var dbConnection = new DatabaseConnection();

                    var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
                    string mailAdress = member.Email;
                    string userId = dbConnection.GetUserIdByMail(mailAdress);

                    if (dbConnection.DidUserBookWebinar(termId, userId))
                    {
                        await turnContext.SendActivityAsync("Das Webinar wurde von Ihnen bereits gebucht.");
                    }
                    else
                    {
                        dbConnection.InsertIntoConnectionTable(termId, userId);

                        await turnContext.SendActivityAsync(MessageFactory.Text("Das Webinar wurde gebucht"), cancellationToken);
                        await turnContext.SendActivityAsync(MessageFactory.Text(terminId), cancellationToken);
                    }
                }
                else if(value.Contains("break"))
                { 
                    await turnContext.SendActivityAsync(MessageFactory.Text("Die Buchung wurde abgebrochen"), cancellationToken); 
                }
                return false;
            }

            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Es gab einen Fehler..."), cancellationToken);
                return false;
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