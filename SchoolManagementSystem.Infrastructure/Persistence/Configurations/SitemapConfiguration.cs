using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class SitemapConfiguration : AuditableEntityConfiguration<Sitemap>
{
    public override void Configure(EntityTypeBuilder<Sitemap> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.Name).HasMaxLength(400).IsRequired();
        entityTypeBuilder.Property(x => x.PageUrl).HasMaxLength(400);
        entityTypeBuilder.Property(x => x.FavIcon).HasMaxLength(100).IsRequired();
        entityTypeBuilder.Property(x => x.ParentId).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.Name, x.PageUrl }).IsUnique();
        // for foreign key relation
        entityTypeBuilder.HasOne(x => x.Parent).WithMany(a => a.SitemapList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_gs_Sitemaps");
    }
}