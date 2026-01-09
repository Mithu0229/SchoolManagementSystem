using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class AuditableEntityConfiguration<T> : CoreEntityConfiguration<T> where T : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.IsActive).HasColumnOrder(90);
        builder.Property(x => x.CreatedById).HasColumnOrder(103).IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnOrder(104).HasDefaultValueSql("GETDATE()").IsRequired();
        builder.Property(x => x.ModifiedById).HasColumnOrder(105);
        builder.Property(x => x.ModifiedDate).HasColumnOrder(106);
    }
}