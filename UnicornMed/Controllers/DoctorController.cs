using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using UnicornMed.Common.Context;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Helpers.JwtAuthenticationManager;
using UnicornMed.Common.Helpers.API.BookingHelper;
using UnicornMed.Common.Helpers.API.DoctorHelper;
using System;

namespace UnicornMed.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorHelper doctorHelper;
        private readonly IBookingHelper bookingHelper;

        public DoctorController(IDoctorHelper doctorHelper, IBookingHelper bookingHelper)
        {
            this.doctorHelper = doctorHelper;
            this.bookingHelper = bookingHelper;
        }

        [AllowAnonymous]
        [HttpGet("doctors")]
        public List<DoctorItem> GetAllDoctors()
        {
            var doctors = doctorHelper.GetDoctors();
            IEnumerable<DoctorItem> doctorItems = doctorHelper.GetDoctorItems(doctors.ToList());
            return doctorItems.ToList();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetDoctor(int id)
        {
            if (!doctorHelper.IdExists(id)) return BadRequest("Doctor does not exist");

            var doctor = doctorHelper.GetDoctor(id);
            DoctorItem doctorItem = doctorHelper.GetDoctorItem(doctor);
            
            return Ok(doctorItem);

        }

        [AllowAnonymous]
        [HttpGet("{id}/slots")]
        public IActionResult GetDoctorAvailability(int id, [FromQuery(Name = "date")] DateTime date)
        {
            if (!doctorHelper.IdExists(id)) return BadRequest("Doctor does not exist");
            Doctor d = doctorHelper.GetDoctor(id);
            AvailabilityItem doctorAvailability = doctorHelper.GetAvailability(d, date);

            return Ok(doctorAvailability);

        }

        [AllowAnonymous]
        [HttpGet("{doctorId}/schedule")]
        public IActionResult GetDoctorSchedule(int doctorId, [FromQuery(Name = "date")] DateTime date)
        {
            if (!doctorHelper.IdExists(doctorId)) return BadRequest("Doctor does not exist");
            Doctor d = doctorHelper.GetDoctor(doctorId);
            ScheduleItem doctorSchedule = doctorHelper.GetSchedule(d, date);

            return Ok(doctorSchedule);

        }

        [AllowAnonymous]
        [HttpGet("all/availability")]
        public IActionResult GetAllAvailability([FromQuery(Name = "date")] DateTime date)
        {
            List<AvailabilityItem> allDoctorAvailability = doctorHelper.GetAllAvailability(date).ToList();

            return Ok(allDoctorAvailability);

        }

        [AllowAnonymous]
        [HttpGet("all/schedule")]
        public List<ScheduleItem> GetAllSchedules([FromQuery(Name = "date")] DateTime date)
        {
            List<ScheduleItem> allDoctorSchedules = doctorHelper.GetAllSchedules(date).ToList();

            return allDoctorSchedules;

        }

        [AllowAnonymous]
        [HttpGet("/mostbooked")]
        public List<ScheduleItem> GetMostBooked([FromQuery(Name = "date")] DateTime date, [FromQuery(Name = "numberToFetch")] int n = 20)
        {
            List<ScheduleItem> mostBookedDoctors = doctorHelper.GetMostBooked(n,date).ToList();

            return mostBookedDoctors;

        }

        [AllowAnonymous]
        [HttpGet("/busy")]
        public IActionResult GetBusyDoctors([FromQuery(Name = "date")] DateTime date)
        {
            List<ScheduleItem> busyDoctors = doctorHelper.GetBusyDoctors(date).ToList();

            return Ok(busyDoctors);

        }

        //[AllowAnonymous]
        //[HttpGet("available")]
        //public IEnumerable<DoctorItem> GetAvailableDoctors([FromQuery(Name = "date")] DateTime date)
        //{
        //    List<DoctorItem> availableDoctors = new List<DoctorItem>();
        //    List<Booking> dayBookings = bookingHelper.GetDayBookings(date).ToList();
        //    List<Doctor> doctors = this.context.Doctors.ToList();
        //    doctors.ForEach(d =>
        //    {
        //        if (doctorHelper.IsAvailable(d.Id, date, dayBookings))
        //        {
        //            availableDoctors.Add(doctorHelper.GetDoctorItem(d));
        //        }
        //    });

        //    return availableDoctors;
        //}
    }
}
