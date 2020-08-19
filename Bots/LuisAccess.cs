using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Bots;
using System;
using System.Collections.Generic;
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

        public async Task<bool> GetRequest(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) =>
            await GetIntent(turnContext, cancellationToken);

        private async Task<bool> GetIntent(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var recognizerResult = await LuisNavigation.RecognizeAsync(turnContext, cancellationToken);
            var topIntent = recognizerResult.GetTopScoringIntent();

            if (topIntent.intent == "Webinar_buchen")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
