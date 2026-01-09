using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class TenantEntityConfiguration<T> : AuditableEntityConfiguration<T> where T : TenantEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.TenantId).HasColumnOrder(107);
    }
}