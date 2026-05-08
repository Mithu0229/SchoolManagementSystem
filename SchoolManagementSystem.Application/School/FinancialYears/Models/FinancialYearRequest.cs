namespace SchoolManagementSystem.Application.School.FinancialYears.Models;

public class FinancialYearRequest
{
    public Guid Id { get; set; }
    public string FinYearName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int FinCode { get; set; }
    public string? Remarks { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
}
