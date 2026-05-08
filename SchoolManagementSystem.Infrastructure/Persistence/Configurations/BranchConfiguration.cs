using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class BranchConfiguration : AuditableEntityConfiguration<Branch>
{
    public override void Configure(EntityTypeBuilder<Branch> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.BranchName).HasMaxLength(250).IsRequired();
        entityTypeBuilder.Property(x => x.BranchAddress).HasMaxLength(1000).IsRequired(false);
        entityTypeBuilder.Property(x => x.ContactPerson).HasMaxLength(200).IsRequired(false);
        entityTypeBuilder.Property(x => x.ContactNumber).HasMaxLength(50).IsRequired(false);
        entityTypeBuilder.Property(x => x.HomeThemeImagePath).HasMaxLength(500).IsRequired(false);
        entityTypeBuilder.Property(x => x.InstituteId).IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.InstituteId, x.BranchName, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Institute).WithMany(x => x.Branches).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.InstituteId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_Branches");
    }
}
