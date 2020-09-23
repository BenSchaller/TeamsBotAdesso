using EchoBot.DatabaseAccess;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using System.CodeDom.Compiler;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class LuisAccess
    {
        public LuisRecognizer LuisNavigation;


        public LuisAccess(LuisRecognizer luisNavigation)
        {
            this.LuisNavigation = luisNavigation;
        }

        public async Task<IdentifiedIntent> GetIntent(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var recognizerResult = await LuisNavigation.RecognizeAsync(turnContext, cancellationToken);
            var topIntent = recognizerResult.GetTopScoringIntent();
            var result = new IdentifiedIntent();

            if (topIntent.intent == "Webinar_buchen")
            {
                result = IdentifiedIntent.WebinarBuchen;
                return result;
            }
            else
            {
                result = IdentifiedIntent.None;
                return result;
            }
        }
    }
}
