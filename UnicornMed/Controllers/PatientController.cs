using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using UnicornMed.Common.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using UnicornMed.Common.Helpers.API.PatientHelper;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Helpers.JwtAuthenticationManager;
using UnicornMed.Common.Helpers.API.ResponseItems;

namespace UnicornMed.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientHelper patientHelper;
        private readonly AppDbContext context;

        public PatientController(AppDbContext context, IPatientHelper patientHelper)
        {
            this.context = context;
            this.patientHelper = patientHelper;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public IEnumerable<Patient> GetAllPatients()
        //{
        //    return this.context.Patients;
        //}

        [AllowAnonymous]
        [HttpGet("history/{id}")]
        public IActionResult GetHistory(int id)
        {
            if (!patientHelper.IdExists(id)) return BadRequest("Patient does not exist");
            Patient patient = patientHelper.GetPatient(id);
            HistoryItem history = patientHelper.GetHistory(patient);
            
            return Ok(history);
        }
    }
}
