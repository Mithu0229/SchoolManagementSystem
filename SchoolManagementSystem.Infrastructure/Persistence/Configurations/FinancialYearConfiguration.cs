using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FinancialYearConfiguration : AuditableEntityConfiguration<FinancialYear>
{
    public override void Configure(EntityTypeBuilder<FinancialYear> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.FinYearName).HasMaxLength(50).IsRequired();
        entityTypeBuilder.Property(x => x.FromDate).IsRequired();
        entityTypeBuilder.Property(x => x.ToDate).IsRequired();
        entityTypeBuilder.Property(x => x.FinCode).IsRequired();
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.Property(x => x.IsCurrent).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.FinYearName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasIndex(x => new { x.FinCode, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_FinancialYears");
    }
}
