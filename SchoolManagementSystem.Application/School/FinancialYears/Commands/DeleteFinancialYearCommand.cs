namespace SchoolManagementSystem.Application.School.FinancialYears.Commands;

public record DeleteFinancialYearCommand(Guid id) : IHttpRequest;
