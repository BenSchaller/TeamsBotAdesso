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
using EchoBot.ConversationStateHandler;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsBot : ActivityHandler
    {
        private ConversationState _conversationState;
        private UserState _userState;
        private IStatePropertyAccessor<ConvState> _conversationStateProperty;
        private IStatePropertyAccessor<Attachment> _webinarCardProperty;
        private Attachment _webinarCard;

        public TeamsBot(LuisRecognizerOptionsV3 optionsLuis, QnAMakerEndpoint endpoint, ConversationState conversationState, UserState userState)
        {
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

            if (conversationStateObj.Webinar)
            {
                var assign = new AssignActivity(turnContext, cancellationToken, EchoBotQnA);
                conversationStateObj.Webinar = await assign.Assigner();

                SaveConvState saveConvState = new SaveConvState(_conversationState, _userState, conversationStateObj);
                await saveConvState.ConvStateSaver(turnContext, cancellationToken);
            }

            else
            {
                //Access the Luis class
                var luisRouting = new LuisAccess(LuisNavigation);

                //Use IdentifiedIntent class to check if Question
                var routing = await luisRouting.GetIntent(turnContext, cancellationToken);

                switch (routing)
                {
                    case IdentifiedIntent.None:

                        var qnaMaker = new QnAMakerAccess(EchoBotQnA);

                        await qnaMaker.AccessQnAMaker(turnContext, cancellationToken);

                        break;

                    case IdentifiedIntent.WebinarBuchen:
                        conversationStateObj.Webinar = true;
                        var saveConvState = new SaveConvState(_conversationState, _userState, conversationStateObj);
                        await saveConvState.ConvStateSaver(turnContext, cancellationToken);

                        var card = new CreateWebinarCard(turnContext);
                        _webinarCard = card.GetWebinarCardFromJson();

                        await turnContext.SendActivityAsync(MessageFactory.Attachment(_webinarCard));

                        var userInformation = new UseUserInformation();

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
    }
}
