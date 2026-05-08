using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class ShiftConfiguration : AuditableEntityConfiguration<Shift>
{
    public override void Configure(EntityTypeBuilder<Shift> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.ShiftName).HasMaxLength(100).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.ShiftName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_Shifts");
    }
}
