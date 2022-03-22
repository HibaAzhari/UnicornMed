using System.Collections.Generic;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.ResponseItems
{
    public class AvailabilityItem
    {
        public DoctorItem Doctor { get; set; }
        public List<SlotItem> Slots { get; set; }

    }
}
