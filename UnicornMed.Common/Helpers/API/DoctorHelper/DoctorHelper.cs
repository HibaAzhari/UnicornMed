using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Context;
using UnicornMed.Common.Helpers.API.BookingHelper;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.DoctorHelper
{
    public class DoctorHelper : IDoctorHelper
    {
        private IBookingHelper bookingHelper;
        private AppDbContext context;
        public DoctorHelper(IBookingHelper bookingHelper, AppDbContext context)
        {
            this.bookingHelper = bookingHelper;
            this.context = context;
        }
        public bool IdExists(int id)
        {
            return context.Doctors.Where(p => p.Id == id).Any();
        }

        public Doctor GetDoctor(int id)
        {
            return context.Doctors.Where(d => d.Id == id).FirstOrDefault();
        }

        public bool EmailExists(string email)
        {
            return context.Doctors.Where(context => context.Email == email).Any();
        }

        public bool IsAvailable(int id, DateTime date)
        {
            // Get doctor's bookings for the day
            var doctorBookings = context.Bookings.Where(b => 
            b.Doctor_Id == id 
            && b.StartTime.Date == date.Date 
            && !b.IsCanceled);

            // Check total hrs
            int bookedHrs = doctorBookings
                .Select(b => b.EndTime.Hour - b.StartTime.Hour)
                .ToList()
                .Sum();

            // Check total patients
            int patients = doctorBookings
                .Count();

            return bookedHrs<8 && patients<12;
        }

        public bool IsFreeAt(int id, DateTime StartTime, DateTime EndTime)
        {
            var bookings = context.Bookings.Where(b => b.Doctor_Id == id).ToList();
            return !bookings.Where(
                b => 
                DateHelper.IsOverlapping(
                    b.StartTime, 
                    b.EndTime,
                    StartTime, 
                    EndTime
                    )
                )
                .Any();
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return context.Doctors;
        }

        public IEnumerable<SlotItem> GetDaySlots(Doctor d, DateTime date)
        {
            List<SlotItem> slots = new List<SlotItem>();
            var bookings = GetDayBookings(d, date).ToList();

            // If Doctor is free all day, return 1 big slot
            if (bookings.Count == 0)
            {
                slots.Add(new SlotItem
                {
                    Start = new DateTime(date.Year, date.Month, date.Day, Constants.DayStart, 0, 0),
                    End = new DateTime(date.Year, date.Month, date.Day, Constants.DayEnd, 0, 0)
                });
                return slots;
            }
            // If there's time before the first booking of the day, handle it 
            if (!(bookings[0].StartTime.Hour == Constants.DayStart))
            {
                slots.Add(new SlotItem
                {
                    Start = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        Constants.DayStart, 0, 0
                    ),
                    End = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        bookings[0].StartTime.Hour, 0, 0
                    )
                });
            }
            // Loop through day's bookings, get free slots
            int i = 0;
            while (!(bookings[i].EndTime.Hour == Constants.DayEnd) && (i < bookings.Count - 1))
            {
                slots.Add(new SlotItem
                {
                    Start = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        bookings[i].EndTime.Hour, 0, 0
                    ),
                    End = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        bookings[i + 1].StartTime.Hour, 0, 0
                    )
                });
                i++;
            }
            // If there's time after the last booking of the day, handle it
            if (!(bookings[bookings.Count - 1].EndTime.Hour == Constants.DayEnd))
            {
                slots.Add(new SlotItem
                {
                    Start = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        bookings[bookings.Count-1].EndTime.Hour, 0, 0
                    ),
                    End = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        Constants.DayEnd, 0, 0
                    )
                });
            }
            return slots;
        }

        public IEnumerable<Booking> GetDayBookings(Doctor d, DateTime date)
        {
            return this.context.Bookings.Where(b => !b.IsCanceled && b.Doctor_Id == d.Id && b.StartTime.Date == date.Date);
        }

        public AvailabilityItem GetAvailability(Doctor doctor, DateTime date)
        {
            DoctorItem doctorItem = GetDoctorItem(doctor);

            List<Booking> doctorBookings = GetDayBookings(doctor, date).ToList();

            List<SlotItem> slots = GetDaySlots(doctor, date).ToList();

            return new AvailabilityItem
            {
                Doctor = doctorItem,
                Slots = slots
            };
        }

        public ScheduleItem GetSchedule(Doctor doctor, DateTime date)
        {
            DoctorItem doctorItem = GetDoctorItem(doctor);

            List<Booking> doctorBookings = GetDayBookings(doctor, date).ToList();

            List<BookingItem> bookings = bookingHelper.GetBookingItems(doctorBookings).ToList();

            return new ScheduleItem
            {
                Doctor = doctorItem,
                Bookings = bookings
            };
        }

        public DoctorItem GetDoctorItem(Doctor d)
        {
            return new DoctorItem
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email
            };
        }

        public IEnumerable<DoctorItem> GetDoctorItems(List<Doctor> doctors)
        {
            List<DoctorItem> doctorItems = new List<DoctorItem>();

            doctors.ForEach(d =>
            {
                doctorItems.Add(new DoctorItem
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Email = d.Email
                });
            });

            return doctorItems;
        }

        public IEnumerable<AvailabilityItem> GetAllAvailability(DateTime date)
        {
            List<Doctor> doctors = GetDoctors().ToList();

            List<AvailabilityItem> allAvailability = new List<AvailabilityItem>();

            doctors.ForEach(d =>
            {
                allAvailability.Add(new AvailabilityItem
                {
                    Doctor = GetDoctorItem(d),
                    Slots = GetDaySlots(d, date).ToList()
                });

            });
            return allAvailability;

        }

        public IEnumerable<ScheduleItem> GetAllSchedules(DateTime date)
        {
            var doctors = GetDoctors().ToList();
            List<ScheduleItem> schedules = new List<ScheduleItem>();

            doctors.ForEach((d) =>
            {
                schedules.Add(GetSchedule(d, date));
            });

            return schedules;
        }

        public IEnumerable<ScheduleItem> GetMostBooked(int numberToFetch, DateTime date)
        {
            // Select .Count() of bookings grouped by doctor
            var bookingCount = context.Bookings
                .Where(b => b.StartTime.Year == date.Year && b.StartTime.Month == date.Month && b.StartTime.Day == date.Day)
                .GroupBy(b => b.Doctor_Id)
                .Select(b => new { DoctorId = b.Key, TotalBookings = b.Count() })
                .OrderBy(r => r.TotalBookings)
                .ToList();

            List<ScheduleItem> mostBookedDoctors = new List<ScheduleItem>();

            // Loop through query result, add as many as requested to mostBookedDoctors
            int i = 0;
            while (i < bookingCount.Count && i < numberToFetch)
            {
                Doctor d = GetDoctor(bookingCount[i].DoctorId);
                mostBookedDoctors.Add(GetSchedule(d, date));
                i++;
            }

            return mostBookedDoctors;
        }

        public IEnumerable<ScheduleItem> GetBusyDoctors(DateTime date, int? bookedLimit = 6)
        {
            List<ScheduleItem> busyDoctors = new List<ScheduleItem>();

            List<Doctor> doctors = GetDoctors().ToList();

            // Loop through doctors
            doctors.ForEach(d =>
            {
                // Get their day's bookings
                var doctorBookings = GetDayBookings(d, date);
                
                // Caclulate total hours
                if (doctorBookings.Any())
                {
                    int bookedHours = doctorBookings
                    .Select(b => b.EndTime.Hour - b.StartTime.Hour)
                    .ToList()
                    .Sum();

                    // Add to busyDoctors if exceeding set limit
                    if (bookedHours >= bookedLimit) busyDoctors.Add(GetSchedule(d, date));
                }
            });

            return busyDoctors;
        }

    }
}
