using System;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace UnicornMed.Common.Models.Database.Bot
{
    public class ConversationReferenceEntity
    {
        private string _upn;
        public string ActivityId { get; set; }
        public string ChannelId { get; set; }
        public string Locale { get; set; }
        public string ServiceUrl { get; set; }
        public string BotId { get; set; }
        public string UserId { get; set; }

        [Key]
        public string UPN
        {
            get => _upn;
            set => _upn = value.ToLower();
        }
        public string Name { get; set; }
        public string AadObjectId { get; set; }
        public string ConversationId { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
    }
}
