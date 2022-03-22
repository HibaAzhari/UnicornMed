using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.Common.Helpers.JwtAuthenticationManager
{
    public interface IJwtAuthenticationManager
    {
        bool AuthenticatePatient(string username, string password);
        bool AuthenticateDoctor(string username, string password);
        bool AuthenticateAdmin(string username, string password);
        string GetToken(string email);

    }
}
