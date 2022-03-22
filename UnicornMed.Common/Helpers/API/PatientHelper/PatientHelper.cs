using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Context;
using UnicornMed.Common.Helpers.API.BookingHelper;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.PatientHelper
{
    public class PatientHelper : IPatientHelper
    {
        private readonly AppDbContext context;
        private readonly IBookingHelper bookingHelper;
        public PatientHelper(AppDbContext context, IBookingHelper bookingHelper)
        {
            this.context = context;
            this.bookingHelper = bookingHelper;
        }

        public bool IdExists(int id)
        {
            return context.Patients.Where(p => p.Id == id).Any();
        }

        public Patient GetPatient(int id)
        {
            return context.Patients.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool EmailExists(string email)
        {
            return context.Patients.Where(context => context.Email == email).Any();
        }

        // Check patient availability at time range
        public bool IsFreeAt(int id, DateTime StartTime, DateTime EndTime)
        {
            // Fetch Patient bookings for the day
            var sameDayBookings = context.Bookings.Where(b => b.Patient_Id == id && b.StartTime.Date == StartTime.Date).ToList();
            // Check for overlap
            return !sameDayBookings.Where(b => DateHelper.IsOverlapping(b.StartTime,b.EndTime,StartTime,EndTime)).Any();
        }

        // Check if patient is already booked with doctor that day
        public bool IsBookedWithDoctor(int pId, int dId, DateTime date)
        {
            return context.Bookings.Where(
                b => b.Patient_Id == pId 
                && b.Doctor_Id == dId 
                && date.Date == b.StartTime.Date 
                && !b.IsCanceled
                ).Any();
        }

        public HistoryItem GetHistory(Patient patient)
        {
            List<Booking> patientBookings = context.Bookings.Where(b => 
                    b.Id == patient.Id 
                    && b.StartTime < DateTime.Now)
                .OrderByDescending(b => b.StartTime)
                .ToList();
            List<BookingItem> bookingItems = bookingHelper.GetBookingItems(patientBookings).ToList();
            return new HistoryItem
            {
                Patient = patient,
                BookingItems = bookingItems
            };
        }
    }
}
