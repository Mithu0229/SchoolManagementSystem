using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities;

public class CashBook : AuditableEntity
{
    public Guid BillMasterId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Debit { get; set; }      // Cash In

    public decimal Credit { get; set; }     // Cash Out

    public decimal Balance { get; set; }

    public string AccountNo { get; set; }

    public string VoucherNo { get; set; }

    public string Particulars { get; set; }

    public BillMaster BillMaster { get; set; }
}
