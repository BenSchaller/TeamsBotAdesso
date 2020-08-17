using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class CreateHeroCard
    {
        public HeroCard FillHeroCardArray(string answer)
        {
            var getObjects = JObject.Parse(answer);

            //Write JSONObject in String
            string title, buttonDescription, buttonUrl, imageUrl;
            title = (string)getObjects["title"];
            buttonDescription = (string)getObjects["buttonDesc"];
            buttonUrl = (string)getObjects["url"];
            imageUrl = (string)getObjects["imgUrl"];

            HeroCard heroCard = CreateHeroCardFromJson(title, buttonDescription, buttonUrl, imageUrl);
            return heroCard;
        }


        public static HeroCard CreateHeroCardFromJson(string title, string buttonDescription, string buttonUrl, string imageUrl)
        {
            HeroCard heroCard = new HeroCard
            {
                Title = title,
            };

            heroCard.Buttons = new List<CardAction>
            {
                new CardAction() { Value = buttonUrl, Title = buttonDescription, Type = ActionTypes.OpenUrl }
            };

            heroCard.Images = new List<CardImage>
            {
                new CardImage( buttonUrl = imageUrl)
            };

            return heroCard;
        }
    }
}
