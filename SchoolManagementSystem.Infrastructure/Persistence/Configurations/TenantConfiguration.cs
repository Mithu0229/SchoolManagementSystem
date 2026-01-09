using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;
public class TenantConfiguration : AuditableEntityConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);
        entityTypeBuilder.Property(x => x.TenantName).IsRequired();
        entityTypeBuilder.Property(x => x.BinNo).IsRequired(false);
        entityTypeBuilder.Property(x => x.TenantEmail).HasMaxLength(200).IsRequired();
        entityTypeBuilder.Property(x => x.PhoneNumber).IsRequired();
        entityTypeBuilder.Property(x => x.Domain).IsRequired(false);
        entityTypeBuilder.Property(x => x.Street).IsRequired(false);
        entityTypeBuilder.Property(x => x.City).IsRequired(false);
        entityTypeBuilder.Property(x => x.Province).IsRequired(false);
        entityTypeBuilder.Property(x => x.PostCode).IsRequired(false);
        entityTypeBuilder.Property(x => x.Reason).IsRequired(false);
        entityTypeBuilder.ToTable("tb_gs_Tenants");
        entityTypeBuilder.HasIndex(x => new { x.TenantEmail, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");

    }
}