using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : AuditableEntityConfiguration<Student>
{
    public override void Configure(EntityTypeBuilder<Student> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.StudentCode).HasMaxLength(50).IsRequired();
        entityTypeBuilder.Property(x => x.StudentName).HasMaxLength(250).IsRequired();
        entityTypeBuilder.Property(x => x.DateOfBirth).IsRequired(false);
        entityTypeBuilder.Property(x => x.Gender).HasMaxLength(20).IsRequired(false);
        entityTypeBuilder.Property(x => x.BloodGroup).HasMaxLength(20).IsRequired(false);
        entityTypeBuilder.Property(x => x.MobileNo).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.Email).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.DOBNo).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.GuardianNID).HasMaxLength(100).IsRequired(false);
        entityTypeBuilder.Property(x => x.FatherName).HasMaxLength(250).IsRequired(false);
        entityTypeBuilder.Property(x => x.MotherName).HasMaxLength(250).IsRequired(false);
        entityTypeBuilder.Property(x => x.GuardianMobileNo).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.PresentAddress).HasMaxLength(1000).IsRequired(false);
        entityTypeBuilder.Property(x => x.PermanentAddress).HasMaxLength(1000).IsRequired(false);
        entityTypeBuilder.Property(x => x.PhotoPath).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.StudentCode, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_Students");
    }
}
