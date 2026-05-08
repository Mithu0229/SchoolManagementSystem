using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class SectionConfiguration : AuditableEntityConfiguration<Section>
{
    public override void Configure(EntityTypeBuilder<Section> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.SectionName).HasMaxLength(100).IsRequired();
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.SectionName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_Sections");
    }
}
