using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class BillDetailConfiguration : AuditableEntityConfiguration<BillDetail>
{
    public override void Configure(EntityTypeBuilder<BillDetail> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.BillMasterId).IsRequired();
        entityTypeBuilder.Property(x => x.FeeTemplateDetailId).IsRequired();
        entityTypeBuilder.Property(x => x.FeeHeadId).IsRequired();
        entityTypeBuilder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.HasOne(x => x.BillMaster).WithMany(x => x.Details).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.BillMasterId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.FeeTemplateDetail).WithMany(x => x.BillDetails).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeTemplateDetailId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.FeeHead).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeHeadId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_BillDetails");
    }
}
