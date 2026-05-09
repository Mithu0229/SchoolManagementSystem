namespace SchoolManagementSystem.Application.School.SchoolStudents.Commands;

public record DeleteStudentCommand(Guid id) : IHttpRequest;
