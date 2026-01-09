using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class UserConfiguration: TenantEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);
        entityTypeBuilder.Property(x => x.FirstName).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.LastName).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.Email).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.PhoneNumber).IsRequired();
        entityTypeBuilder.Property(x => x.Password).IsRequired();
        entityTypeBuilder.Property(x => x.UserType).IsRequired();
        entityTypeBuilder.Property(x => x.Address).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.Email, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Tenant).WithMany(a => a.TenantUserList).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_gs_Users");
    }
}
