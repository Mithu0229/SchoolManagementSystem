using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations.Students;
public class LocalGuardianInfoConfiguration : CoreEntityConfiguration<LocalGuardianInfo>
{
    public override void Configure(EntityTypeBuilder<LocalGuardianInfo> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.RelationToStudent).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.Phone).IsRequired();
        entityTypeBuilder.ToTable("tb_sl_LocalGuardianInfo");
        entityTypeBuilder.HasOne(x => x.StudentInfo).WithOne(a => a.LocalGuardianInfo).OnDelete(DeleteBehavior.Restrict).HasForeignKey<LocalGuardianInfo>(x => x.StudentInfoId).OnDelete(DeleteBehavior.Cascade);
    }
}
