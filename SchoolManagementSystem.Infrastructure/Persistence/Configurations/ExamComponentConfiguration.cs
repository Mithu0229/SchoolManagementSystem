using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class ExamComponentConfiguration : AuditableEntityConfiguration<ExamComponent>
{
    public override void Configure(EntityTypeBuilder<ExamComponent> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.TermName).HasMaxLength(50).IsRequired();
        entityTypeBuilder.Property(x => x.ComponentName).HasMaxLength(100).IsRequired();
        entityTypeBuilder.Property(x => x.MaxMarks).HasPrecision(5, 2).IsRequired();
        entityTypeBuilder.Property(x => x.OrderIndex).HasDefaultValue(0);

        entityTypeBuilder.HasOne(x => x.AcademicSession)
            .WithMany()
            .HasForeignKey(x => x.AcademicSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.ToTable("tb_sch_ExamComponents");
    }
}
