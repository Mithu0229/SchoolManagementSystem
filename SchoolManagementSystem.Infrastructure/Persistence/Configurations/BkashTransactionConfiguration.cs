using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class BkashTransactionConfiguration : AuditableEntityConfiguration<BkashTransaction>
{
    public override void Configure(EntityTypeBuilder<BkashTransaction> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.Date).IsRequired();
        entityTypeBuilder.Property(x => x.TransactionType).HasMaxLength(10).IsRequired();
        entityTypeBuilder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.FromNumber).HasMaxLength(20).IsRequired();
        entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(500);

        entityTypeBuilder.ToTable("tb_sch_BkashTransactions");
    }
}
