using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.DoctorHelper
{
    public interface IDoctorHelper
    {
        public bool IdExists(int id);
        public Doctor GetDoctor(int id);
        public bool EmailExists(string email);
        public bool IsAvailable(int id, DateTime date);
        public bool IsFreeAt(int id, DateTime StartTime, DateTime EndTime);
        public IEnumerable<Doctor> GetDoctors();
        public IEnumerable<SlotItem> GetDaySlots(Doctor doctor, DateTime date);
        public IEnumerable<Booking> GetDayBookings(Doctor d, DateTime date);
        public AvailabilityItem GetAvailability(Doctor doctor, DateTime date);
        public ScheduleItem GetSchedule(Doctor doctor, DateTime date);
        public DoctorItem GetDoctorItem(Doctor doctor);
        public IEnumerable<DoctorItem> GetDoctorItems(List<Doctor> d);
        public IEnumerable<AvailabilityItem> GetAllAvailability(DateTime date);
        public IEnumerable<ScheduleItem> GetAllSchedules(DateTime date);
        public IEnumerable<ScheduleItem> GetMostBooked(int numberToFetch, DateTime date);
        public IEnumerable<ScheduleItem> GetBusyDoctors(DateTime date, int? bookedLimit = 6);
    }
}
