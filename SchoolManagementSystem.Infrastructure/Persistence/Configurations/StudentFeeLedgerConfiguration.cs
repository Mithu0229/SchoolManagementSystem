using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class StudentFeeLedgerConfiguration : AuditableEntityConfiguration<StudentFeeLedger>
{
    public override void Configure(EntityTypeBuilder<StudentFeeLedger> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.EntryDate).IsRequired();
        entityTypeBuilder.Property(x => x.StudentId).IsRequired();
        entityTypeBuilder.Property(x => x.AdmissionId).IsRequired();
        entityTypeBuilder.Property(x => x.BranchId).IsRequired();
        entityTypeBuilder.Property(x => x.ClassId).IsRequired();
        entityTypeBuilder.Property(x => x.FinancialYearId).IsRequired();
        entityTypeBuilder.Property(x => x.MonthNo).IsRequired();
        entityTypeBuilder.Property(x => x.YearNo).IsRequired();
        entityTypeBuilder.Property(x => x.FeeAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.CollectionAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.DueAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.MemoNo).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.VoucherCode).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.Property(x => x.IsCancelled).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.StudentId, x.AdmissionId, x.FinancialYearId, x.MonthNo, x.YearNo, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Student).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.Admission).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne<Branch>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne<AcademicClass>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.FinancialYear).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_StudentFeeLedgers");
    }
}
