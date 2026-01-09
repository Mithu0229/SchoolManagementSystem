using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class CoreEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : CoreEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.Property(x => x.Id).HasColumnOrder(0).HasDefaultValueSql("NEWID()").IsRequired();
      
        builder.Property(x => x.IsDeleted).HasColumnOrder(100).HasDefaultValue(false);
        builder.Property(x => x.DeletedById).HasColumnOrder(101);
        builder.Property(x => x.DeletedDate).HasColumnOrder(102);

    }
}
