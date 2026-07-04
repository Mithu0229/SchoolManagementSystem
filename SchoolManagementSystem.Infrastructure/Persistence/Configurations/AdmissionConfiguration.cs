using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class AdmissionConfiguration : AuditableEntityConfiguration<Admission>
{
    public override void Configure(EntityTypeBuilder<Admission> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.AdmissionDate).IsRequired();
        entityTypeBuilder.Property(x => x.StudentId).IsRequired();
        entityTypeBuilder.Property(x => x.BranchId).IsRequired();
        entityTypeBuilder.Property(x => x.AcademicSessionId).IsRequired();
        entityTypeBuilder.Property(x => x.ClassId).IsRequired();
        entityTypeBuilder.Property(x => x.SectionId).IsRequired(false);
        entityTypeBuilder.Property(x => x.ShiftId).IsRequired(false);
        entityTypeBuilder.Property(x => x.GroupId).IsRequired(false);
        entityTypeBuilder.Property(x => x.RollNo).HasMaxLength(50).IsRequired();
        entityTypeBuilder.Property(x => x.IsPassed).IsRequired();
        entityTypeBuilder.Property(x => x.IsCancelled).IsRequired();
        entityTypeBuilder.HasOne(x => x.Student)
           .WithMany()
           .HasForeignKey(x => x.StudentId)
           .OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasIndex(x => new { x.RollNo, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Student).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.Branch).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.AcademicSession).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.AcademicSessionId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.Class).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.Section).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.Shift).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ShiftId).OnDelete(DeleteBehavior.Restrict);
        //entityTypeBuilder.HasOne(x => x.Group).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_Admissions");
    }
}
