using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FeeTemplateDetailConfiguration : AuditableEntityConfiguration<FeeTemplateDetail>
{
    public override void Configure(EntityTypeBuilder<FeeTemplateDetail> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.FeeTemplateId).IsRequired();
        entityTypeBuilder.Property(x => x.FeeHeadId).IsRequired();
        entityTypeBuilder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.FeeTemplateId, x.FeeHeadId, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.FeeTemplate).WithMany(x => x.Details).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeTemplateId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.FeeHead).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.FeeHeadId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_FeeTemplateDetails");
    }
}
