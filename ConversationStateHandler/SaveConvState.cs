using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.ConversationStateHandler
{
    public class SaveConvState
    {
        private ConvState conversationStateObj;
        private ConversationState _conversationState;
        private UserState _userState;
        private IStatePropertyAccessor<ConvState> _conversationStateProperty;

        public SaveConvState(ConversationState conversationState, UserState userState, ConvState convState)
        {
            conversationStateObj = convState;
            _conversationState = conversationState;
            _userState = userState;
            _conversationStateProperty = _conversationState.CreateProperty<ConvState>(nameof(ConvState));

        }

        public async Task ConvStateSaver(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _conversationStateProperty.SetAsync(turnContext, conversationStateObj);
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}
