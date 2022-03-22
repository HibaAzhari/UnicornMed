using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.ResponseItems
{
    public class HistoryItem
    {
        public Patient Patient { get; set; }

        public List<BookingItem> BookingItems { get; set; }
    }
}
