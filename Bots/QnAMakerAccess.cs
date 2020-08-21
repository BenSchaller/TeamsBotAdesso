using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
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
                await turnContext.SendActivityAsync(attachment, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker hat keine Antwort gefunden."), cancellationToken);
            }
        }
    }
}
