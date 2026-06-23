using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class BillMasterConfiguration : AuditableEntityConfiguration<BillMaster>
{
    public override void Configure(EntityTypeBuilder<BillMaster> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.AdmissionId).IsRequired();
        entityTypeBuilder.Property(x => x.BillMonth).IsRequired();
        entityTypeBuilder.Property(x => x.BillYear).IsRequired();
        entityTypeBuilder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.HasIndex(x => new { x.AdmissionId, x.BillMonth, x.BillYear, x.IsDeleted }).IsUnique().HasFilter("\"IsDeleted\" = 0");
        entityTypeBuilder.HasOne(x => x.Admission).WithMany().OnDelete(DeleteBehavior.Restrict).HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Restrict);
        entityTypeBuilder.ToTable("tb_sch_BillMasters");
    }
}
