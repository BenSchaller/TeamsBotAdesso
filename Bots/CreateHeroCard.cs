using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace EchoBot.Bots
{
    public class CreateHeroCard
    {
        public HeroCard FillHeroCard(string answer)
        {
            HeroCard heroCard = JsonConvert.DeserializeObject<HeroCard>(answer);
            return heroCard;
        }
    }
}
