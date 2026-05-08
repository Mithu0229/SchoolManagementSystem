using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FeeCollectionConfiguration : AuditableEntityConfiguration<FeeCollection>
{
    public override void Configure(EntityTypeBuilder<FeeCollection> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.CollectionDate).IsRequired();
        entityTypeBuilder.Property(x => x.MemoNo).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.StudentId).IsRequired();
        entityTypeBuilder.Property(x => x.AdmissionId).IsRequired();
        entityTypeBuilder.Property(x => x.BranchId).IsRequired();
        entityTypeBuilder.Property(x => x.FinancialYearId).IsRequired();
        entityTypeBuilder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.PaidAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.DueAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.PaymentMode).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.Property(x => x.IsCancelled).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.MemoNo, x.IsDeleted }).IsUnique(false).HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne<Student>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne<Admission>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne<Branch>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne<FinancialYear>().WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_FeeCollections");
    }
}
