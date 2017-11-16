using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VA.DAL
{
    public class VAContext : DbContext
    {
        public VAContext() : base("VAContext")
        {
            Database.SetInitializer<VAContext>(null);
        }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<AppointmentTimeSlot> AppointmentTimeSlot { get; set; }
        public DbSet<Clinic> Clinic { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<VAService> Service { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<Specie> Specie { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }

        public static void Clear<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Member>().HasMany(i => i.Pets).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<Member>().HasMany(i => i.Appointments).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<VAService>().HasMany(i => i.Appointments).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<TimeBlock>().HasMany(i => i.AppointmentTimeBlock).WithRequired().WillCascadeOnDelete(false);
        }
       
    }
}