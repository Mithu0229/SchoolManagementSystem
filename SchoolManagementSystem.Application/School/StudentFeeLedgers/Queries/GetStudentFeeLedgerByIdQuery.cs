namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Queries;

public record GetStudentFeeLedgerByIdQuery(Guid Id) : IHttpRequest;
