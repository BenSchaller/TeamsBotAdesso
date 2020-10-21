using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.Luis;
using EchoBot.Bots;
using EchoBot.Logic;
using Microsoft.Bot.Builder.Teams;
using Newtonsoft.Json.Linq;
using AdaptiveCards;
using EchoBot.ConversationState;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsBot : ActivityHandler
    {
        private ConversationState _conversationState;
        private UserState _userState;
        private IStatePropertyAccessor<ConvState> _conversationStateProperty;

        public TeamsBot(LuisRecognizerOptionsV3 optionsLuis, QnAMakerEndpoint endpoint, ConversationState conversationState, UserState userState)
        {
            //Connects to QnAMakerEnpoint for each turn
            LuisNavigation = new LuisRecognizer(optionsLuis);
            EchoBotQnA = new QnAMaker(endpoint);
            _conversationState = conversationState;
            _userState = userState;
            _conversationStateProperty = _conversationState.CreateProperty<ConvState>(nameof(ConvState));
        }


        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var conversationStateObj = await _conversationStateProperty.GetAsync(turnContext, () => new ConvState());

            var userStateProp = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userStateObj = await userStateProp.GetAsync(turnContext, () => new UserProfile());

            UseUserInformation userInformation = new UseUserInformation();
            var activity = turnContext.Activity;

            //Access the Luis class
            var luisRouting = new LuisAccess(LuisNavigation);

            //Use IdentifiedIntent class to check if Question
            var routing = await luisRouting.GetIntent(turnContext, cancellationToken);
            if (conversationStateObj.Webinar)
            {
                var assign = new AssignActivity(turnContext, cancellationToken, LuisNavigation, EchoBotQnA);
                await assign.Assigner();
                await turnContext.SendActivityAsync("du bist noch in der Webinar Klasse");
            }

            else if (conversationStateObj.QnA)
            {
                await turnContext.SendActivityAsync("du bist noch in der QnA Klasse");

            }
            else
            {
                switch (routing)
                {
                    case IdentifiedIntent.None:
                        await _conversationStateProperty.SetAsync(turnContext, conversationStateObj);

                        var qnaMaker = new QnAMakerAccess(EchoBotQnA);

                        await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);

                        break;

                    case IdentifiedIntent.WebinarBuchen:

                        conversationStateObj.Webinar = true;
                        await _conversationStateProperty.SetAsync(turnContext, conversationStateObj);
                        await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);

                        var webinarCard = new CreateWebinarCard(turnContext);
                        var card = webinarCard.GetWebinarCardFromJson();

                        await turnContext.SendActivityAsync(MessageFactory.Attachment(card));

                        var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
                        userInformation.CreateNewUserEntry(member);

                        break;
                }
            }

            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            //Willkommensnachricht beim ersten Login/Registrierung des Bots
            var welcomeText = "Herzlich Willkommen!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        //Construct connection to Microsoft Cognitive Services
        public LuisRecognizer LuisNavigation { get; private set; }

        public QnAMaker EchoBotQnA { get; private set; }

        //public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        //{
        //    //Returns the raw Context/Echo Kommentar

        //    UseUserInformation userInformation = new UseUserInformation();

        //var activity = turnContext.Activity;
        //    switch (turnContext.Activity.Type)
        //    {
        //        case ActivityTypes.ConversationUpdate:
        //            var welcomeText = "Herzlich Willkommen!";
        //            IList<ChannelAccount> membersAdded = new List<ChannelAccount>();
        //            foreach (var member in membersAdded)
        //            {
        //                if (member.Id != turnContext.Activity.Recipient.Id)
        //                {
        //                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
        //                }
        //            }
        //            break;

        //        case ActivityTypes.Message:

        //            if (activity.Text != null && activity.Value == null)
        //            {
        //                //Access the Luis class
        //                var luisRouting = new LuisAccess(LuisNavigation);

        //                //Use IdentifiedIntent class to check if Question

        //                if (await luisRouting.GetIntent(turnContext, cancellationToken) == IdentifiedIntent.None)
        //                {
        //                    var qnaMaker = new QnAMakerAccess(EchoBotQnA);
        //                    await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);
        //                }
        //                else
        //                {
        //                    var webinarCard = new CreateWebinarCard(turnContext);
        //                    var card = webinarCard.GetWebinarCardFromJson();

        //                    await turnContext.SendActivityAsync(MessageFactory.Attachment(card));


        //                    var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
        //                    userInformation.CreateNewUserEntry(member);
        //                }

        //                break;

        //            }
        //            else if (string.IsNullOrEmpty(activity.Text) && activity.Value != null)
        //            {
        //                await turnContext.SendActivityAsync(MessageFactory.Text("Ich bin nun im Buchungszweig"), cancellationToken);
        //                var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

        //                ConnectUserWithTermin connectUser = new ConnectUserWithTermin();
        //                connectUser.WriteInConnectionTable(member);

        //                break;
        //            }
        //            else
        //            {
        //                await turnContext.SendActivityAsync(MessageFactory.Text("Es gab einen Fehler..."), cancellationToken);

        //                break;
        //            }

        //    };
        //}


    }
}
