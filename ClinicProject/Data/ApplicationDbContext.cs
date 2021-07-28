using ClinicProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Specialization>().HasData(
                new {Id=(long)1, SpecializationName="Sp1"},
                new {Id=(long)2, SpecializationName="Sp2"},
                new {Id=(long)3, SpecializationName = "Sp3" }
                );
        }


        public DbSet <Doctor> Doctors { get; set; }
        public DbSet <Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Typee> Types { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
    }
}
