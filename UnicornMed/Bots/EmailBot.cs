using UnicornMed.Common.Context;
using UnicornMed.BotLibrary.Helpers;
using UnicornMed.Common.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnicornMed.Bots
{
    public class EmailBot : TeamsActivityHandler
    {
        protected readonly IConversationReferenceHelper _conversationReferenceHelper;
        protected readonly AppDbContext context;
        protected readonly IEmailPromptHelper emailPromptHelper;
        //protected readonly HttpClient client = new HttpClient();
        public EmailBot(IConversationReferenceHelper conversationReferencesHelper, AppDbContext context, IEmailPromptHelper emailPromptHelper)
        {
            _conversationReferenceHelper = conversationReferencesHelper;
            this.context = context;
            this.emailPromptHelper = emailPromptHelper;
        }

        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
        }
        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            ConversationReference botConRef = activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, activity.From.Id, cancellationToken);

            if (activity.Action.Equals("add"))
                await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
            else if (activity.Action.Equals("remove"))
                await _conversationReferenceHelper.DeleteConversationRefrenceAsync(botConRef, currentMember);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome!"), cancellationToken);
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Value != null)
            {
                dynamic data = turnContext.Activity.Value;

                await this.emailPromptHelper.RegisterEmail(data.email, data.Name, data.dept);

                await turnContext.SendActivityAsync("Successfully Registered:\n\nName: "+data.Name+"\n\nDepartment: "+ data.dept +"\n\nAlternate Email: "+data.email);
            }
        }
    }
}
