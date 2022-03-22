using System;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.ResponseItems
{
    public class BookingItem
    {
        public int Id { get; set; }

        public Doctor Doctor { get; set; }

        public Patient Patient { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsCanceled { get; set; }

    }
}
