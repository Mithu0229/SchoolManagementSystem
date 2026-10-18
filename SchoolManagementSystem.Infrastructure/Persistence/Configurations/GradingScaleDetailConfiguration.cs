using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class GradingScaleDetailConfiguration : CoreEntityConfiguration<GradingScaleDetail>
{
    public override void Configure(EntityTypeBuilder<GradingScaleDetail> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.GradingScaleId).IsRequired();
        entityTypeBuilder.Property(x => x.MinMark).HasPrecision(5, 2).IsRequired();
        entityTypeBuilder.Property(x => x.MaxMark).HasPrecision(5, 2).IsRequired();
        entityTypeBuilder.Property(x => x.LetterGrade).HasMaxLength(10).IsRequired();
        entityTypeBuilder.Property(x => x.GradePoint).HasPrecision(4, 2).IsRequired();
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.OrderIndex).HasDefaultValue(0);

        entityTypeBuilder.ToTable("tb_sch_GradingScaleDetails");
    }
}
