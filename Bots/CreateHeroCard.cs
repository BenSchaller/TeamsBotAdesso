﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class CreateHeroCard
    {
        public HeroCard FillHeroCard(string answer)
        {
            HeroCard heroCard = JsonConvert.DeserializeObject<HeroCard>(answer);
            return heroCard;


            //    var getObjects = JObject.Parse(answer);
            //    //Write JSONObject in String
            //    string title, buttonDescription, buttonUrl, imageUrl;
            //    title = (string)getObjects["Title"];
            //    buttonDescription = (string)getObjects["ButtonDescription"];
            //    buttonUrl = (string)getObjects["Value"];
            //    imageUrl = (string)getObjects["url"];

            //    HeroCard heroCard = CreateHeroCardFromJson(title, buttonDescription, buttonUrl, imageUrl);
            //    return heroCard;
            //}


            //public static HeroCard CreateHeroCardFromJson(string title, string buttonDescription, string buttonUrl, string imageUrl)
            //{
            //    HeroCard heroCard = new HeroCard
            //    {
            //        Title = title,
            //    };
            //    heroCard.Buttons = new List<CardAction>
            //    {
            //        new CardAction() { Value = buttonUrl, Title = buttonDescription, Type = ActionTypes.OpenUrl }
            //    };

            //    heroCard.Images = new List<CardImage>
            //    {
            //        new CardImage( buttonUrl = imageUrl)
            //    };

            //    return heroCard;
        }
    }
}
