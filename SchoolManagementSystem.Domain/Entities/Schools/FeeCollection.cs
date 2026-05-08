namespace SchoolManagementSystem.Domain.Entities;

public class FeeCollection : AuditableEntity
{
    public DateTime CollectionDate { get; set; }
    public string? MemoNo { get; set; }

    public Guid StudentId { get; set; }
    public Guid AdmissionId { get; set; }
    public Guid BranchId { get; set; }
    public Guid FinancialYearId { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }

    public string? PaymentMode { get; set; } // Cash, Bank, Bkash
    public string? Remarks { get; set; }
    public bool IsCancelled { get; set; }

    public ICollection<FeeCollectionDetail> Details { get; set; }
}