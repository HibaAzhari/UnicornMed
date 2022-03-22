using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

using UnicornMed.Common.Helpers.API.BookingHelper;
using UnicornMed.Common.Helpers.API.DoctorHelper;
using UnicornMed.Common.Helpers.API.PatientHelper;
using UnicornMed.Common.Context;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingHelper bookingHelper;
        private readonly IDoctorHelper doctorHelper;
        private readonly IPatientHelper patientHelper;
        private readonly AppDbContext context;

        public BookingController(AppDbContext context, IBookingHelper bookingHelper, IDoctorHelper doctorHelper, IPatientHelper patientHelper)
        {
            this.context = context;
            this.bookingHelper = bookingHelper;
            this.doctorHelper = doctorHelper;
            this.patientHelper = patientHelper;
        }

        [AllowAnonymous]
        [HttpPost("new")]
        public async Task<IActionResult> Book([FromBody] Booking booking)
        {
            if (!doctorHelper.IdExists(booking.Doctor_Id)) return BadRequest("Doctor does not exist");
            if (!patientHelper.IdExists(booking.Patient_Id)) return BadRequest("Patient does not exist");
            if (!doctorHelper.IsAvailable(booking.Doctor_Id, booking.StartTime)) return BadRequest("Doctor Unavailable");
            if (!patientHelper.IsFreeAt(booking.Patient_Id, booking.StartTime, booking.EndTime)) return BadRequest("Patient is booked at this time");
            if (!doctorHelper.IsFreeAt(booking.Doctor_Id, booking.StartTime, booking.EndTime)) return BadRequest("Doctor is booked at this time");
            if (patientHelper.IsBookedWithDoctor(booking.Patient_Id, booking.Doctor_Id, booking.StartTime)) return BadRequest("Patient already booked with this doctor on this day");
            if (!bookingHelper.IsValidBookingTiming(booking.StartTime, booking.EndTime)) return BadRequest("Invalid booking timing");
            context.Add(booking);
            await context.SaveChangesAsync();
            return Ok(booking);
        }

        [AllowAnonymous]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!bookingHelper.IdExists(id)) return BadRequest("Boooking does not exist");
            if (bookingHelper.IsCancelled(id)) return BadRequest("Booking Already cancelled");
            if (bookingHelper.IsInThePast(id)) return BadRequest("Cannot cancel past bookings");
            var booking = bookingHelper.GetBooking(id);
            booking.IsCanceled = true;
            await context.SaveChangesAsync();
            return Ok(booking);
        }

        [AllowAnonymous]
        [HttpGet("{id}/view")]
        public IActionResult GetBooking(int id)
        {
            if (!bookingHelper.IdExists(id)) return BadRequest("Booking does not exist");
            Booking booking = bookingHelper.GetBooking(id);
            BookingItem bookingItem = bookingHelper.GetBookingItem(booking);
            
            return Ok(bookingItem);
        }
    }
}
