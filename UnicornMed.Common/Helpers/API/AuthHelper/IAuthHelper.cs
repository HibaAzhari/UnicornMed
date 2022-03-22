using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Models.Database.API;

namespace UnicornMed.Common.Helpers.API.AuthHelper
{
    public interface IAuthHelper
    {
        public Patient EncodePatientPassword(UserItem user);
        public Admin EncodeAdminPassword(UserItem user);
        public Doctor EncodeDoctorPassword(UserItem user);
        public bool IsValidEmail(string email);
        public string AuthenticatePatient(UserCred userCred);
        public string AuthenticateDoctor(UserCred userCred);
        public string AuthenticateAdmin(UserCred userCred);
        public bool ValidUserCred(UserItem user);
    }
}
