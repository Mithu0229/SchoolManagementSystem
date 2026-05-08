using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class StudentGroupConfiguration : AuditableEntityConfiguration<StudentGroup>
{
    public override void Configure(EntityTypeBuilder<StudentGroup> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.GroupName).HasMaxLength(100).IsRequired();
        entityTypeBuilder.Property(x => x.GroupDetails).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.GroupName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_StudentGroups");
    }
}
