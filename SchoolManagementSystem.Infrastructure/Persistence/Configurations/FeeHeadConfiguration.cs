using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FeeHeadConfiguration : AuditableEntityConfiguration<FeeHead>
{
    public override void Configure(EntityTypeBuilder<FeeHead> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.FeeHeadName).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.IsMonthly).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.FeeHeadName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_FeeHeads");
    }
}
