using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations.Students;

public class GuardianInfoConfiguration : CoreEntityConfiguration<GuardianInfo>
{
    public override void Configure(EntityTypeBuilder<GuardianInfo> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.FatherName).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.FatherNationalIdNo).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.FatherMobile).IsRequired();
        entityTypeBuilder.Property(x => x.MotherName).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.MotherNationalIdNo).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.MotherMobile).IsRequired();
        entityTypeBuilder.ToTable("tb_sl_GuardianInfo");
        entityTypeBuilder.HasOne(x => x.StudentInfo).WithOne(a => a.GuardianInfo).OnDelete(DeleteBehavior.Restrict).HasForeignKey<GuardianInfo>(x => x.StudentInfoId).OnDelete(DeleteBehavior.Cascade);
    }
}

