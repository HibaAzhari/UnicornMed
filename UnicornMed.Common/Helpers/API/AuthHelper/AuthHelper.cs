using UnicornMed.Common.Helpers;
using UnicornMed.Common.Helpers.API.ResponseItems;
using UnicornMed.Common.Helpers.JwtAuthenticationManager;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Context;

namespace UnicornMed.Common.Helpers.API.AuthHelper
{
    public class AuthHelper : IAuthHelper
    {
        //private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        private readonly AppDbContext context;

        public AuthHelper(AppDbContext context)
        {
            //this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.context = context;
        }
        public Patient EncodePatientPassword(UserItem user)
        {
            var passwordHash = PasswordHelper.EncryptPlainTextToCipherText(user.Password);
            Patient encodedPatient = new Patient(user.FirstName, user.LastName, user.Email, passwordHash);
            return encodedPatient;
        }
        public Admin EncodeAdminPassword(UserItem user)
        {
            var passwordHash = PasswordHelper.EncryptPlainTextToCipherText(user.Password);
            Admin encodedAdmin = new Admin(user.FirstName, user.LastName, user.Email, passwordHash);
            return encodedAdmin;
        }
        public Doctor EncodeDoctorPassword(UserItem user)
        {
            var passwordHash = PasswordHelper.EncryptPlainTextToCipherText(user.Password);
            Doctor encodedDoctor = new Doctor(user.FirstName, user.LastName, user.Email, passwordHash);
            return encodedDoctor;
        }

        public string AuthenticatePatient(UserCred userCred)
        {
            string token = null;
            //if (jwtAuthenticationManager.AuthenticatePatient(userCred.email, userCred.password))
            //{
            //    token = jwtAuthenticationManager.GetToken(userCred.email);
            //}

            return token;
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public string AuthenticateDoctor(UserCred userCred)
        {
            string token = null;

            //if (jwtAuthenticationManager.AuthenticateDoctor(userCred.email, userCred.password))
            //{
            //    token = jwtAuthenticationManager.GetToken(userCred.email);
            //}

            return token;
        }

        public string AuthenticateAdmin(UserCred userCred)
        {
            string token = null;
            //if (jwtAuthenticationManager.AuthenticateAdmin(userCred.email, userCred.password))
            //{
            //    token = jwtAuthenticationManager.GetToken(userCred.email);
            //}

            return token;
        }

        public bool ValidUserCred(UserItem user)
        {
            var validEmail = IsValidEmail(user.Email);
            var validPassword = !user.Password.Contains(" ");
            return validEmail && validPassword;
        }
    }
}
