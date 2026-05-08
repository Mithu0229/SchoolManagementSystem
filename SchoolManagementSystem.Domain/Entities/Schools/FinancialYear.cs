namespace SchoolManagementSystem.Domain.Entities;

public class FinancialYear : AuditableEntity
{
    public required string FinYearName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int FinCode { get; set; }
    public string? Remarks { get; set; }
    public bool IsCurrent { get; set; }
}
