using HumanResources.Models.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Context
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(1000);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PersonalPermit> PersonalPermits { get; set; }
        public DbSet<AdvancePayment> AdvancePayments { get; set; }
        public DbSet<PersonalAdvance> PersonalAdvances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<PersonalExpense> PersonalExpenses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Idea> Ideas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalPermit>().HasOne(x => x.User).WithMany(x => x.PersonalPermits).HasForeignKey(x => x.PersonalID);
            modelBuilder.Entity<PersonalAdvance>().HasOne(x => x.User).WithMany(x => x.PersonalAdvances).HasForeignKey(x => x.PersonalID);
            modelBuilder.Entity<PersonalExpense>().HasOne(x => x.User).WithMany(x => x.PersonalExpenses).HasForeignKey(x => x.PersonalID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
