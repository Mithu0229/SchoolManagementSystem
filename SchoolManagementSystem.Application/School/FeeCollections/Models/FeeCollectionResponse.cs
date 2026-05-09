namespace SchoolManagementSystem.Application.School.FeeCollections.Models;

public class FeeCollectionResponse
{
    public Guid Id { get; set; }
    public DateTime CollectionDate { get; set; }
    public string? MemoNo { get; set; }
    public Guid StudentId { get; set; }
    public string? StudentName { get; set; }
    public Guid AdmissionId { get; set; }
    public Guid BranchId { get; set; }
    public Guid FinancialYearId { get; set; }
    public string? FinYearName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string? PaymentMode { get; set; }
    public string? Remarks { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsActive { get; set; }
    public IList<FeeCollectionDetailResponse> Details { get; set; } = new List<FeeCollectionDetailResponse>();
}

public class FeeCollectionDetailResponse
{
    public Guid Id { get; set; }
    public Guid FeeCollectionId { get; set; }
    public Guid FeeHeadId { get; set; }
    public string? FeeHeadName { get; set; }
    public Guid MonthNo { get; set; }
    public Guid YearNo { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
}
