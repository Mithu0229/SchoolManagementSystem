namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;

public class StudentFeeLedgerResponse
{
    public Guid Id { get; set; }
    public DateTime EntryDate { get; set; }
    public Guid StudentId { get; set; }
    public string? StudentName { get; set; }
    public Guid AdmissionId { get; set; }
    public Guid BranchId { get; set; }
    public Guid ClassId { get; set; }
    public Guid FinancialYearId { get; set; }
    public string? FinYearName { get; set; }
    public int MonthNo { get; set; }
    public int YearNo { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal CollectionAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string? MemoNo { get; set; }
    public string? VoucherCode { get; set; }
    public string? Remarks { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsActive { get; set; }
}
