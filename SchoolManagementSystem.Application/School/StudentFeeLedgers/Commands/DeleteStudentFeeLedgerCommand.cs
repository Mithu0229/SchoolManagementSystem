namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;

public record DeleteStudentFeeLedgerCommand(Guid id) : IHttpRequest;
