using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Persistence.Configurations;

public class BankBookConfiguration : AuditableEntityConfiguration<BankBook>
{
    public override void Configure(EntityTypeBuilder<BankBook> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder.Property(x => x.BillMasterId).IsRequired();
        entityTypeBuilder.Property(x => x.TransactionDate).IsRequired();
        entityTypeBuilder.Property(x => x.Debit).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.Credit).HasColumnType("decimal(18,2)").IsRequired();
        entityTypeBuilder.Property(x => x.Balance).HasColumnType("decimal(18,2)").IsRequired();
        
        entityTypeBuilder.Property(x => x.BankName).HasMaxLength(200);
        entityTypeBuilder.Property(x => x.AccountNo).HasMaxLength(100);
        entityTypeBuilder.Property(x => x.TransactionNo).HasMaxLength(100);
        entityTypeBuilder.Property(x => x.TransactionType).IsRequired();
        entityTypeBuilder.Property(x => x.VoucherNo).HasMaxLength(100);
        entityTypeBuilder.Property(x => x.Particulars).HasMaxLength(500);

        entityTypeBuilder.HasOne(x => x.BillMaster)
            .WithMany()
            .HasForeignKey(x => x.BillMasterId)
            .OnDelete(DeleteBehavior.Restrict);

        entityTypeBuilder.ToTable("tb_sch_BankBooks");
    }
}
