using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.ResponseItems
{
    public class ScheduleItem
    {
        public DoctorItem Doctor { get; set; }

        public List<BookingItem> Bookings { get; set; }
    }
}
