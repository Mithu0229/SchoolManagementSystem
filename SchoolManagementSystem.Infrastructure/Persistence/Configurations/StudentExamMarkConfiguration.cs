using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class StudentExamMarkConfiguration : AuditableEntityConfiguration<StudentExamMark>
{
    public override void Configure(EntityTypeBuilder<StudentExamMark> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.MarksObtained).HasPrecision(5, 2).IsRequired();
        entityTypeBuilder.Property(x => x.IsAbsent).HasDefaultValue(false);
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(250).IsRequired(false);

        entityTypeBuilder.HasOne(x => x.AcademicSession)
            .WithMany()
            .HasForeignKey(x => x.AcademicSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.Class)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.Section)
            .WithMany()
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasOne(x => x.ExamComponent)
            .WithMany()
            .HasForeignKey(x => x.ExamComponentId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.HasIndex(x => new { x.AcademicSessionId, x.StudentId, x.SubjectId, x.ExamComponentId, x.IsDeleted })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = 0");

        entityTypeBuilder.ToTable("tb_sch_StudentExamMarks");
    }
}
