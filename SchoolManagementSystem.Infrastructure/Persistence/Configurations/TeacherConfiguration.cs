using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : AuditableEntityConfiguration<Teacher>
{
    public override void Configure(EntityTypeBuilder<Teacher> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        entityTypeBuilder.ToTable("tb_sch_Teacheres");
    }
}