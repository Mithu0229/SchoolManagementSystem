using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Entities.Students;
using SchoolManagementSystem.Infrastructure.Persistence.Configurations;
using System.Reflection;

namespace SchoolManagementSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentInfo> StudentInfo { get; set; }
        public DbSet<GuardianInfo> GuardianInfo { get; set; }
        public DbSet<LocalGuardianInfo> LocalGuardianInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new DivisionConfiguration());

            //base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // This is just for design-time migrations
        //        // The actual connection string will be configured in Program.cs
        //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CleanArchitectureDb;Trusted_Connection=True;");
        //    }
        //}
    }
}
