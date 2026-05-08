using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class AcademicSessionConfiguration : AuditableEntityConfiguration<AcademicSession>
{
    public override void Configure(EntityTypeBuilder<AcademicSession> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.SessionName).HasMaxLength(50).IsRequired();
        entityTypeBuilder.Property(x => x.FromDate).IsRequired();
        entityTypeBuilder.Property(x => x.ToDate).IsRequired();
        entityTypeBuilder.Property(x => x.IsCurrent).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.SessionName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.ToTable("tb_sch_AcademicSessions");
    }
}
