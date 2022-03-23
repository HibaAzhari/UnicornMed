using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using UnicornMed.Common.Models;
using UnicornMed.Common.Models.Database.API;
using UnicornMed.Common.Models.Database.Bot;

namespace UnicornMed.Common.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ConversationReferenceEntity> ConversationReferenceEntities { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }



        // public DbSet<ConversationReference> converationReferences { get; set; }
    }
}
