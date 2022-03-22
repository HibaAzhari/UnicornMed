using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.Common.Models.Database.API
{
    public class Booking
    {
        public int Id { get; set; }
        public int Doctor_Id { get; set; }
        public int Patient_Id { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public bool IsCanceled { get; set; } = false;

        public List<Booking> Bookings { get; set; }
    }
}
