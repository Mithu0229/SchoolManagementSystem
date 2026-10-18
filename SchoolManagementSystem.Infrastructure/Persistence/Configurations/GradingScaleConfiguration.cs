using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class GradingScaleConfiguration : AuditableEntityConfiguration<GradingScale>
{
    public override void Configure(EntityTypeBuilder<GradingScale> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.ScaleName).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.Property(x => x.IsDefault).HasDefaultValue(false);

        entityTypeBuilder.HasMany(x => x.Details)
            .WithOne(x => x.GradingScale)
            .HasForeignKey(x => x.GradingScaleId)
            .OnDelete(DeleteBehavior.Cascade);

        entityTypeBuilder.ToTable("tb_sch_GradingScales");
    }
}
