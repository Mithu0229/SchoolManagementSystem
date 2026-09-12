using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities;

public class BillMaster : AuditableEntity
{
    public Guid AdmissionId { get; set; }

    public int BillMonth { get; set; }
    public int BillYear { get; set; }
    public string? VoucherNo { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal CollectionAmount { get; set; }
    public decimal DueAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public bool IsPaid { get; set; }
    public TransactionType TransactionType { get; set; }

    public Admission Admission { get; set; }

    public ICollection<BillDetail> Details { get; set; }
}
