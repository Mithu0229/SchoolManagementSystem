using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities;

public class BankBook : AuditableEntity
{
    public Guid BillMasterId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Debit { get; set; }      // Deposit

    public decimal Credit { get; set; }     // Withdrawal

    public decimal Balance { get; set; }

    public string BankName { get; set; }

    public string AccountNo { get; set; }

    public string TransactionNo { get; set; }

    public TransactionType TransactionType { get; set; } // Bank/Bkash

    public string VoucherNo { get; set; }

    public string Particulars { get; set; }

    public BillMaster BillMaster { get; set; }
}
