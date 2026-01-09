using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class RoleMenuConfiguration : TenantEntityConfiguration<RoleMenu>
{
    public override void Configure(EntityTypeBuilder<RoleMenu> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);
        entityTypeBuilder.Property(x => x.SitemapId).IsRequired();
        entityTypeBuilder.Property(x => x.RoleId).IsRequired();
        entityTypeBuilder.Property(x => x.CanView).HasDefaultValue(false);
        entityTypeBuilder.Property(x => x.CanAdd).HasDefaultValue(false);
        entityTypeBuilder.Property(x => x.CanEdit).HasDefaultValue(false);
        entityTypeBuilder.Property(x => x.CanDelete).HasDefaultValue(false);
        entityTypeBuilder.Property(x => x.TenantId).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.SitemapId, x.RoleId, x.TenantId, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Sitemap).WithMany(a => a.RoleMenuList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.SitemapId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.Role).WithMany(a => a.RoleMenuList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_gs_RoleMenus");
    }
}