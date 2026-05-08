namespace SchoolManagementSystem.Application.School.FinancialYears.Queries;

public record GetFinancialYearListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
