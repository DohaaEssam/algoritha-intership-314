using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Infrastructure_Layer.DbContext
{
    public class DbEntities : IdentityDbContext<ApplicationUser>
    {
        public DbEntities()
        {
        }
        public DbEntities(DbContextOptions<DbEntities> options) : base(options)
        {
        }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<DiscountCode> DiscountCode { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Time> Time { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=DBs; Integrated Security = True");
            base.OnConfiguring(optionsBuilder);

        }

    }
}
