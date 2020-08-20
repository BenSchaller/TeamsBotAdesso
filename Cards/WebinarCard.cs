using AdaptiveCards;
using AdaptiveExpressions;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.NumberWithUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Cards
{
    public class WebinarCard
    {
        public Attachment CreateWebinarCard()
        {
            var card = new AdaptiveCard();
            card.Body.Add(new AdaptiveTextBlock() { Text = "Webinartermine", Size = AdaptiveTextSize.Medium, Weight = AdaptiveTextWeight.Bolder });
            card.Body.Add(new AdaptiveChoiceSetInput()
            {
                Id = "WebinarNr",
                Style = AdaptiveChoiceInputStyle.Compact,
                Choices = new List<AdaptiveChoice>(new[]
            {
                new AdaptiveChoice(){ Title ="14.05.2020", Value = "1"},
                new AdaptiveChoice(){ Title ="14.06.2020", Value = "2"},
                new AdaptiveChoice(){ Title ="14.07.2020", Value = "3"}

            })
            });
            card.Body.Add(new AdaptiveTextBlock() { Text = "Name:", Size = AdaptiveTextSize.Medium, Weight = AdaptiveTextWeight.Bolder });
            card.Body.Add(new AdaptiveTextInput() { Style = AdaptiveTextInputStyle.Text, Id = "Name" });
            card.Actions.Add(new AdaptiveSubmitAction() { Title = "Anmelden" });

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            
        }
    }
}
