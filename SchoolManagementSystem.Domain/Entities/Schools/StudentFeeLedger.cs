namespace SchoolManagementSystem.Domain.Entities;

public class StudentFeeLedger : AuditableEntity
{
    public DateTime EntryDate { get; set; }

    public Guid StudentId { get; set; }
    public Guid AdmissionId { get; set; }
    public Guid BranchId { get; set; }
    public Guid ClassId { get; set; }
    public Guid FinancialYearId { get; set; }

    public int MonthNo { get; set; } // 1 = January
    public int YearNo { get; set; }

    public decimal FeeAmount { get; set; }
    public decimal CollectionAmount { get; set; }
    public decimal DueAmount { get; set; }

    public string? MemoNo { get; set; }
    public string? VoucherCode { get; set; }
    public string? Remarks { get; set; }

    public bool IsCancelled { get; set; }

    public Student Student { get; set; }
    public Admission Admission { get; set; }
    public FinancialYear FinancialYear { get; set; }
}