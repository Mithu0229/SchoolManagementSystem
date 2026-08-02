using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Entities.Students;
using SchoolManagementSystem.Domain.Entities.Schools;
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
        public DbSet<Institute> Institute { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<FinancialYear> FinancialYear { get; set; }
        public DbSet<AcademicSession> AcademicSession { get; set; }
        public DbSet<AcademicClass> AcademicClass { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<StudentGroup> StudentGroup { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Admission> Admission { get; set; }
        public DbSet<FeeHead> FeeHead { get; set; }
        public DbSet<FeeTemplate> FeeTemplate { get; set; }
        public DbSet<FeeTemplateDetail> FeeTemplateDetail { get; set; }
        public DbSet<StudentFeeLedger> StudentFeeLedger { get; set; }
        public DbSet<FeeCollection> FeeCollection { get; set; }
        public DbSet<FeeCollectionDetail> FeeCollectionDetail { get; set; }
        public DbSet<BillMaster> BillMaster { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
        public DbSet<AttendanceDevice> AttendanceDevices { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<SMSHistory> SMSHistories { get; set; }
        public DbSet<BkashTransaction> BkashTransactions { get; set; }
        public DbSet<CashBook> CashBooks { get; set; }
        public DbSet<BankBook> BankBooks { get; set; }

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

    public class AttendanceDbContext : DbContext
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<IclockTransaction> IclockTransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IclockTransaction>()
                .ToTable("iclock_transaction");

            base.OnModelCreating(modelBuilder);
        }
    }
}
