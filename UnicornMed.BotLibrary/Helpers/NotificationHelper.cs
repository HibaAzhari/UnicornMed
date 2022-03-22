using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnicornMed.Common.Context;

namespace UnicornMed.BotLibrary.Helpers
{
    public interface INotificationHelper
    {
        public Task SendMessage(string userId, string message);
    }

    public class NotificationHelper : INotificationHelper
    {
        protected readonly AppDbContext context;
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IConfiguration _configuration;

        private string _activityId;

        public NotificationHelper(AppDbContext context, IBotFrameworkHttpAdapter adapter, IConfiguration _configuration)
        {
            this.context = context;
            this._adapter = adapter;
            this._configuration = _configuration;
        }

        public async Task SendMessage(string userId, string message)
        {
            var conRef = context.ConversationReferenceEntities.Where(r => r.UserId == userId).FirstOrDefault();

            string botId = _configuration["MicrosoftAppId"];

            if (conRef != null)
            {
                ConversationReference reference = new ConversationReference()
                {
                    Conversation = new ConversationAccount()
                    {
                        Id = conRef.ConversationId
                    },
                    ServiceUrl = conRef.ServiceUrl,
                };

                await ((BotAdapter)_adapter).ContinueConversationAsync(
                       botId,
                       reference,
                       async (context, token) =>
                       {
                           IMessageActivity messageActivity = MessageFactory.Text(message);
                           await BotCallback(messageActivity, context, token);

                       },
                       default);
            }
        }

        private async Task BotCallback(IMessageActivity message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(message);
        }
    }
}
