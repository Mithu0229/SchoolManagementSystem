using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations.Students;
public class StudentInfoConfiguration : TenantEntityConfiguration<StudentInfo>
{
    public override void Configure(EntityTypeBuilder<StudentInfo> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);
        entityTypeBuilder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.Gender).IsRequired();
        entityTypeBuilder.Property(x => x.DateOfBirth).IsRequired();
        entityTypeBuilder.Property(x => x.PlaceOfBirth).IsRequired();
        entityTypeBuilder.Property(x => x.BirthCertificateNo).IsRequired();
        entityTypeBuilder.Property(x => x.BloodGroup).IsRequired();
        entityTypeBuilder.Property(x => x.PermanentAddress).HasMaxLength(1000).IsRequired();
        entityTypeBuilder.Property(x => x.PresentAddress).HasMaxLength(1000).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.FullName, x.IsDeleted }).IsUnique(false).HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sl_StudentInfo");
    }
}

