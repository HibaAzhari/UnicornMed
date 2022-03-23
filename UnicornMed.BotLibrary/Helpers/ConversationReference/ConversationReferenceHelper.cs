using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Context;
using UnicornMed.Common.Models.Database.Bot;

namespace UnicornMed.BotLibrary.Helpers
{
    public class ConversationReferenceHelper : IConversationReferenceHelper
    {
        private readonly AppDbContext context;
        
        public ConversationReferenceHelper(AppDbContext context)
        {
            this.context = context;
        }
        
        public async Task AddorUpdateConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member)
        {
            try
            {
                var entity = ConvertConversationReferenceForDB(reference, member);

                if(!context.ConversationReferenceEntities.Where(r => r.UPN == entity.UPN).Any()) context.Add(entity);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            
        }

        public async Task DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member)
        {
            var entity = ConvertConversationReferenceForDB(reference, member);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<ConversationReferenceEntity> GetConversationRefrenceAsync(string upn)
        {
            return context.ConversationReferenceEntities.Single(r => r.UPN == upn);
        }

        private ConversationReferenceEntity ConvertConversationReferenceForDB(ConversationReference reference, TeamsChannelAccount currentMember)
        {
            return new ConversationReferenceEntity
            {
                UPN = currentMember.UserPrincipalName,
                Name = currentMember.Name,
                AadObjectId = currentMember.AadObjectId,
                UserId = currentMember.Id,
                ActivityId = reference.ActivityId,
                BotId = reference.Bot.Id,
                ChannelId = reference.ChannelId,
                ConversationId = reference.Conversation.Id,
                Locale = reference.Locale,
                RowKey = currentMember.UserPrincipalName,
                ServiceUrl = reference.ServiceUrl,
                PartitionKey = "ConversationReference"
            };
        }
    }
}
