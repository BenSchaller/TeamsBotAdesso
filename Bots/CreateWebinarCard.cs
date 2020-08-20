using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using EchoBot.Cards;
using AdaptiveCards;
using System.IO;

namespace EchoBot.Bots
{
    public class CreateWebinarCard
    {
        public Attachment GetWebinarCardFromJson()
        {
            Attachment card = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText("Cards\\WebinarCard.json")).Card
            };
            return card;
        }
    }
}
