using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicornMed.Common.Models
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AltEmail { get; set; }
        public string Department { get; set; }
    }
}
