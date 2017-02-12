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
        public VAContext() : base
            ("VAContext")
        {

        }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<PetType> PetType { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Member>().HasMany(i => i.Pets).WithRequired().WillCascadeOnDelete(false);
        }
    }
}