using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class UserRoleConfiguration : TenantEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.UserId).IsRequired();
        entityTypeBuilder.Property(x => x.RoleId).IsRequired();
        entityTypeBuilder.Property(x => x.TenantId).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.UserId, x.RoleId, x.TenantId, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.User).WithMany(a => a.UserRoleList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.Role).WithMany(a => a.UserRoleList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_gs_UserRoles");
    }
}
