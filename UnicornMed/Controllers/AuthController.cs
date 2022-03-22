using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnicornMed.Common.Context;
using System.Linq;
using UnicornMed.Common.Helpers.API.AuthHelper;
using UnicornMed.Common.Helpers.API.PatientHelper;
using UnicornMed.Common.Helpers.API.DoctorHelper;
using UnicornMed.Common.Helpers.API.AdminHelper;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Helpers.API.ResponseItems;


namespace UnicornMed.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthHelper authHelper;
        private readonly IPatientHelper patientHelper;
        private readonly IAdminHelper adminHelper;
        private readonly IDoctorHelper doctorHelper;
        private readonly AppDbContext context;

        public AuthController(AppDbContext context, IAuthHelper authHelper, IPatientHelper patientHelper, IAdminHelper adminHelper, IDoctorHelper doctorHelper)
        {
            this.authHelper = authHelper;
            this.patientHelper = patientHelper;
            this.adminHelper = adminHelper;
            this.doctorHelper = doctorHelper;
            this.context = context;
        }

        [AllowAnonymous]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserItem user)
        {
            if (!authHelper.ValidUserCred(user)) return BadRequest();
            if (adminHelper.EmailExists(user.Email)) return BadRequest("Email taken");
            var encodedAdmin = authHelper.EncodeAdminPassword(user);
            context.Add(encodedAdmin);
            await context.SaveChangesAsync();
            return Ok("Successfully Registered " + user.Email);
        }

        [AllowAnonymous]
        [HttpPost("register/doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] UserItem user)
        {
            if (!authHelper.ValidUserCred(user)) return BadRequest();
            if (doctorHelper.EmailExists(user.Email)) return BadRequest("Email taken");
            var encodedDoctor = authHelper.EncodeDoctorPassword(user);
            if (encodedDoctor == null) return StatusCode(500);
            context.Add(encodedDoctor);
            await context.SaveChangesAsync();
            return Ok("Successfully Registered " + user.Email);
        }

        [AllowAnonymous]
        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] UserItem user)
        {
            if (!authHelper.ValidUserCred(user)) return BadRequest();
            if (patientHelper.EmailExists(user.Email)) return BadRequest("Email taken");
            var encodedPatient = authHelper.EncodePatientPassword(user);
            if (encodedPatient == null) return StatusCode(500);
            context.Add(encodedPatient);
            await context.SaveChangesAsync();
            return Ok("Successfully Registered " + user.Email);
        }

        [AllowAnonymous]
        [HttpPost("login/admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] UserCred user)
        {
            if (!adminHelper.EmailExists(user.email)) return BadRequest("User not registered");
            var token = authHelper.AuthenticateAdmin(user);
            if (token == null)
                return Unauthorized("Incorrect credentials");
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login/doctor")]
        public async Task<IActionResult> LoginDoctor([FromBody] UserCred user)
        {
            if (!doctorHelper.EmailExists(user.email)) return BadRequest("User not registered");
            var token = authHelper.AuthenticateDoctor(user);
            if (token == null)
                return Unauthorized("Incorrect credentials");
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login/patient")]
        public async Task<IActionResult> LoginPatient([FromBody] UserCred user)
        {
            if (!patientHelper.EmailExists(user.email)) return BadRequest("User not registered");
            var token = authHelper.AuthenticatePatient(user);
            if (token == null)
                return Unauthorized("Incorrect credentials");
            return Ok(token);
        }
    }
}
