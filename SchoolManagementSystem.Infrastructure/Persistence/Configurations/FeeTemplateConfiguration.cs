using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class FeeTemplateConfiguration : AuditableEntityConfiguration<FeeTemplate>
{
    public override void Configure(EntityTypeBuilder<FeeTemplate> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.TemplateName).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.ClassId).IsRequired();
        entityTypeBuilder.Property(x => x.GroupId).IsRequired(false);
        entityTypeBuilder.Property(x => x.ShiftId).IsRequired(false);
        entityTypeBuilder.HasIndex(x => new { x.TemplateName, x.ClassId, x.GroupId, x.ShiftId, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Class).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.Group).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.HasOne(x => x.Shift).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.ShiftId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_FeeTemplates");
    }
}
