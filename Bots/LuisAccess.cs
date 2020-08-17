using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class LuisAccess : ActivityHandler
    {
        public LuisRecognizer LuisNavigation;

        public LuisAccess(LuisRecognizer luisNavigation)
        {
            this.LuisNavigation = luisNavigation;
        }

        public async Task GetRequest(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await GetIntent(turnContext, cancellationToken);
        }

        private async Task GetIntent(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var recognizerResult = await LuisNavigation.RecognizeAsync(turnContext, cancellationToken);
            var topIntent = recognizerResult.GetTopScoringIntent();

            await turnContext.SendActivityAsync(MessageFactory.Text(topIntent.intent), cancellationToken);
        }
    }
}
