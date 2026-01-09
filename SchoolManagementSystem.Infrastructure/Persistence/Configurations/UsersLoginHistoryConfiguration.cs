using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class UsersLoginHistoryConfiguration : AuditableEntityConfiguration<UsersLoginHistory>
{
    public override void Configure(EntityTypeBuilder<UsersLoginHistory> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);
        entityTypeBuilder.Property(x => x.OperatingSystem).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.IP).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.MAC).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.NetworkType).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.ToTable("tb_gs_UsersLoginHistory");
    }
}
