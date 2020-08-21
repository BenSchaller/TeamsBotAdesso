using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class QnAMakerAccess : ActivityHandler
    {
        public QnAMaker EchoBotQnA { get; private set; }

        public QnAMakerAccess(QnAMaker echoBotQnA)
        {
            EchoBotQnA = echoBotQnA;
        }


        public async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Send it to LUIS

            //Send it to the QnAMaker
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);

            //Result from QnAMaker?
            if (results.Any())
            {
                var resultFirst = results.First();
                CreateHeroCard heroCard = new CreateHeroCard();

                var attachment = MessageFactory.Attachment(heroCard.FillHeroCard(resultFirst.Answer).ToAttachment());

                var getObjects = JObject.Parse(resultFirst.Answer);
                //Write JSONObject in String
                string title, buttonDescription, buttonUrl, imageUrl;
                title = (string)getObjects["Title"];
                buttonDescription = (string)getObjects["ButtonDescription"];
                buttonUrl = (string)getObjects["Value"];
                imageUrl = (string)getObjects["url"];
                await turnContext.SendActivityAsync(MessageFactory.Text("Titel: ", title), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("buttonDescription: ", buttonDescription), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("buttonUrl: ", buttonUrl), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("imageUrl: ", imageUrl), cancellationToken);

                await turnContext.SendActivityAsync(attachment, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker hat keine Antwort gefunden."), cancellationToken);
            }
        }
    }
}
