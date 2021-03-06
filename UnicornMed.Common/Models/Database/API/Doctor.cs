using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.Common.Models.Database.API
{
    [Table("Doctors")]
    public class Doctor
    {
        public Doctor(string firstName, string lastName, string email, string passwordHash)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
