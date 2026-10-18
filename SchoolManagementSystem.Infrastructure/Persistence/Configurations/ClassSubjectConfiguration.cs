using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class ClassSubjectConfiguration : AuditableEntityConfiguration<ClassSubject>
{
    public override void Configure(EntityTypeBuilder<ClassSubject> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.ClassId).IsRequired();
        entityTypeBuilder.Property(x => x.SubjectId).IsRequired();
        entityTypeBuilder.Property(x => x.OrderIndex).HasDefaultValue(0);

        entityTypeBuilder.HasOne(x => x.Class)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.ToTable("tb_sch_ClassSubjects");
    }
}
