using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.PatientHelper
{
    public interface IPatientHelper
    {
        public bool IdExists(int id);
        public Patient GetPatient(int id);
        public bool EmailExists(string email);
        public bool IsFreeAt(int id, DateTime StartTime, DateTime EndTime);
        public bool IsBookedWithDoctor(int pId, int dId, DateTime date);
        public HistoryItem GetHistory(Patient patient);

    }
}
