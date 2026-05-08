using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FeeCollectionDetailConfiguration : AuditableEntityConfiguration<FeeCollectionDetail>
{
    public override void Configure(EntityTypeBuilder<FeeCollectionDetail> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.FeeCollectionId).IsRequired();
        entityTypeBuilder.Property(x => x.FeeHeadId).IsRequired();
        entityTypeBuilder.Property(x => x.MonthNo).IsRequired();
        entityTypeBuilder.Property(x => x.YearNo).IsRequired();
        entityTypeBuilder.Property(x => x.FeeAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.PaidAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.DueAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.HasOne(x => x.FeeCollection).WithMany(x => x.Details).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeCollectionId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.FeeHead).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeHeadId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_FeeCollectionDetails");
    }
}
