namespace SchoolManagementSystem.Application.School.SchoolStudents.Queries;

public record GetStudentByIdQuery(Guid Id) : IHttpRequest;
