using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class InstituteConfiguration : AuditableEntityConfiguration<Institute>
{
    public override void Configure(EntityTypeBuilder<Institute> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.InstituteName).HasMaxLength(250).IsRequired();
        entityTypeBuilder.Property(x => x.Address).HasMaxLength(1000).IsRequired(false);
        entityTypeBuilder.Property(x => x.ContactNo).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.Email).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.LogoPath).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.InstituteName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_Institutes");
    }
}
