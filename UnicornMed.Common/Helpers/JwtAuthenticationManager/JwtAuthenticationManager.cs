using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using UnicornMed.Common.Context;
using UnicornMed.Common.Models.Database.API;
using System.Security.Principal;

namespace UnicornMed.Common.Helpers.JwtAuthenticationManager
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;
        private readonly AppDbContext context;

        public JwtAuthenticationManager(string key, AppDbContext context)
        {
            this.key = key;
            this.context = context;
        }

        public bool AuthenticatePatient(string email, string password)
        {
            Patient patient = context.Patients.Where(x => x.Email == email).FirstOrDefault();
            if (patient == null || !(PasswordHelper.DecryptCipherTextToPlainText(patient.PasswordHash) == password))
            {
                return false;
            }
            return true;
        }

        public bool AuthenticateDoctor(string email, string password)
        {
            Doctor doctor = context.Doctors.Where(x => x.Email == email).FirstOrDefault();
            if (doctor == null || !(PasswordHelper.DecryptCipherTextToPlainText(doctor.PasswordHash) == password))
            {
                return false;
            }
            return true;
        }

        public bool AuthenticateAdmin(string email, string password)
        {
            Admin admin = context.Admins.Where(x => x.Email == email).FirstOrDefault();
            if (admin == null || !(PasswordHelper.DecryptCipherTextToPlainText(admin.PasswordHash) == password))
            {
                return false;
            }
            return true;
        }
        public string GetToken(string email)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF32.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //private bool ValidateToken(string authToken)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = GetValidationParameters();

        //    SecurityToken validatedToken;
        //    IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
        //    return true;
        //}

        //private TokenValidationParameters GetValidationParameters()
        //{
        //    return new TokenValidationParameters()
        //    {
        //        ValidateLifetime = false,
        //        ValidateAudience = false,
        //        ValidateIssuer = false,
        //        ValidIssuer = "Sample",
        //        ValidAudience = "Sample",
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
        //    };
        //}
    }
}
