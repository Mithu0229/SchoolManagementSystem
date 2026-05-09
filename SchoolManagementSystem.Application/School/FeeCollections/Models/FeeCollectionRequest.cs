namespace SchoolManagementSystem.Application.School.FeeCollections.Models;

public class FeeCollectionRequest
{
    public Guid Id { get; set; }
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
    public string? PaymentMode { get; set; }
    public string? Remarks { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsActive { get; set; }
    public IList<FeeCollectionDetailRequest> Details { get; set; } = new List<FeeCollectionDetailRequest>();
}

public class FeeCollectionDetailRequest
{
    public Guid Id { get; set; }
    public Guid FeeHeadId { get; set; }
    public Guid MonthNo { get; set; }
    public Guid YearNo { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
}
