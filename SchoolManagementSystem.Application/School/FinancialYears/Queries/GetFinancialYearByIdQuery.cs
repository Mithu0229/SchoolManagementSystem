namespace SchoolManagementSystem.Application.School.FinancialYears.Queries;

public record GetFinancialYearByIdQuery(Guid Id) : IHttpRequest;
