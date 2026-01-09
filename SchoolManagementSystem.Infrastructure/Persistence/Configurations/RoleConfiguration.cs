using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : TenantEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.RoleName).HasMaxLength(80).IsRequired();
        entityTypeBuilder.Property(x => x.Description).IsRequired(false);
        entityTypeBuilder.Property(x => x.DeleteRequestedOn).IsRequired(false);
        entityTypeBuilder.Property(x => x.TenantId).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.TenantId, x.RoleName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Tenant).WithMany(a => a.TenantRoleList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_gs_Roles");
    }
}