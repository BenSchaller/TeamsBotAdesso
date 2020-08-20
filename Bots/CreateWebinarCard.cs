using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using EchoBot.Cards;


namespace EchoBot.Bots
{
    public class CreateWebinarCard
    {
        public Attachment GetWebinarCard()
        {
            WebinarCard card = new WebinarCard();
            var webinarCard = card.CreateWebinarCard();
            return webinarCard;
        }
    }
}
