namespace SchoolManagementSystem.Application.School.StudentGroups.Queries;

public record GetStudentGroupByIdQuery(Guid Id) : IHttpRequest;
