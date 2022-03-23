using Microsoft.Bot.Schema.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using UnicornMed.Common.Models.Database.Bot;

namespace UnicornMed.BotLibrary.Helpers
{
    public interface IConversationReferenceHelper
    {
        Task AddorUpdateConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
        Task DeleteConversationRefrenceAsync(ConversationReference reference, TeamsChannelAccount member);
        Task<ConversationReferenceEntity> GetConversationRefrenceAsync(string upn);

    }
}
