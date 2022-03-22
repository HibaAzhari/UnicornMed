using System;
using System.Collections.Generic;
using System.Linq;
using UnicornMed.Common.Context;
using UnicornMed.Common.Helpers.JwtAuthenticationManager;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Helpers.API.ResponseItems;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Helpers.API.DoctorHelper;

namespace UnicornMed.Common.Helpers.API.BookingHelper
{
    public class BookingHelper : IBookingHelper
    {
        private readonly AppDbContext context;

        public BookingHelper(AppDbContext context)
        {
            this.context = context;
        }

        public Booking GetBooking(int id)
        {
            return context.Bookings.Where(b => b.Id == id).FirstOrDefault();
        }

        public bool IdExists(int id)
        {
            return context.Bookings.Where(b => b.Id == id).Any();
        }

        public bool IsCancelled(int id)
        {
            return context.Bookings.Where(b => b.Id == id).First().IsCanceled;
        }

        public bool IsValidBookingTiming(DateTime StartTime, DateTime EndTime)
        {
            return StartTime >= DateTime.Now
                && EndTime > StartTime
                && StartTime.Hour >= Constants.DayStart
                && EndTime <= new DateTime(StartTime.Year,StartTime.Month,StartTime.Day,Constants.DayEnd,0,0)
                && (EndTime - StartTime).TotalMinutes <= Constants.MaxBookingLengthMinutes
                && (EndTime - StartTime).TotalMinutes >= Constants.MinBookingLengthMinutes;
        }

        public bool IsInThePast(int id)
        {
            return context.Bookings.Where(b => b.Id == id).First().EndTime < DateTime.Now;
        }

        public IEnumerable<Booking> GetDayBookings(DateTime date)
        {
            return this.context.Bookings.Where(b => !b.IsCanceled && b.StartTime.Date == date.Date);
        }

        public BookingItem GetBookingItem(Booking b)
        {
            var Doctor = context.Doctors.Where(d => d.Id == b.Id).FirstOrDefault();
            var Patient = context.Patients.Where(p => p.Id == b.Id).FirstOrDefault();

            return new BookingItem
            {
                Id = b.Id,
                Doctor = Doctor,
                Patient = Patient,
                Start = b.StartTime,
                End = b.EndTime
            };
        }

        public IEnumerable<BookingItem> GetBookingItems(IEnumerable<Booking> bookings)
        {
            List<BookingItem> bookingItems = new List<BookingItem>();

            bookings.ToList().ForEach(b => {
                bookingItems.Add(GetBookingItem(b));
            });

            return bookingItems;
        }

        //public IEnumerable<Booking> GetDoctorBookings(int doctorId, DateTime date)
        //{
        //    return this.context.Bookings
        //        .Where(b => !b.IsCanceled && b.Doctor_Id == doctorId && b.StartTime.Date == date.Date)
        //        .ToList();
        //}

        //public IEnumerable<Booking> GetDoctorBookings(int doctorId)
        //{
        //    return this.context.Bookings
        //        .Where(b => b.Doctor_Id == doctorId && !b.IsCanceled)
        //        .ToList();
        //}

        //public IEnumerable<BookingItem> GetBookingItems(int id)
        //{
        //    List<Booking> bookings = GetDoctorBookings(id).ToList();

        //    List<BookingItem> bookingItems = new List<BookingItem>();


        //    bookings.ToList().ForEach(b => {
        //        bookingItems.Add(
        //            new BookingItem
        //            {
        //                Id = b.Id,
        //                DoctorId = b.Doctor_Id,
        //                PatientId = b.Patient_Id,
        //                Start = b.StartTime,
        //                End = b.EndTime
        //            });
        //    });

        //    return bookingItems;
        //}

        

    }
}
