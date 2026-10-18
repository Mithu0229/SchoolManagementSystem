using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : AuditableEntityConfiguration<Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.SubjectName).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.SubjectCode).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.ToTable("tb_sch_Subjects");
    }
}
