using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class AcademicClassConfiguration : AuditableEntityConfiguration<AcademicClass>
{
    public override void Configure(EntityTypeBuilder<AcademicClass> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.ClassName).HasMaxLength(100).IsRequired();
        entityTypeBuilder.Property(x => x.ClassDetails).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.ClassName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_AcademicClasses");
    }
}
