using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class AttendanceDeviceConfiguration : AuditableEntityConfiguration<AttendanceDevice>
{
    public override void Configure(EntityTypeBuilder<AttendanceDevice> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.DeviceNo).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.CardNo).HasMaxLength(150).IsRequired();
        entityTypeBuilder.Property(x => x.DtPunchDate).HasColumnType("date").IsRequired();
        entityTypeBuilder.Property(x => x.DtPunchTime).IsRequired();
        entityTypeBuilder.Property(x => x.InOut).IsRequired();
        
        entityTypeBuilder.ToTable("tb_sch_AttendanceDevices");
    }
}
