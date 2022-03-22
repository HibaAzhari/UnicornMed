using System;
using System.Collections.Generic;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Helpers.API.DoctorHelper;
using UnicornMed.Common.Helpers.API.ResponseItems;

namespace UnicornMed.Common.Helpers.API.BookingHelper
{
    public interface IBookingHelper
    {
        public Booking GetBooking(int id);
        public bool IdExists(int id);

        public bool IsCancelled(int id);

        public bool IsValidBookingTiming(DateTime StartTime, DateTime EndTime);

        public bool IsInThePast(int id);

        public IEnumerable<Booking> GetDayBookings(DateTime date);

        public BookingItem GetBookingItem(Booking b);

        public IEnumerable<BookingItem> GetBookingItems(IEnumerable<Booking> bookings);

    }
}
