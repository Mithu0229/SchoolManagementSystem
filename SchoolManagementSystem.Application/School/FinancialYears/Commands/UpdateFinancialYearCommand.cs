using SchoolManagementSystem.Application.School.FinancialYears.Models;

namespace SchoolManagementSystem.Application.School.FinancialYears.Commands;

public class UpdateFinancialYearCommand : IHttpRequest
{
    public FinancialYearRequest FinancialYear { get; set; }
}
