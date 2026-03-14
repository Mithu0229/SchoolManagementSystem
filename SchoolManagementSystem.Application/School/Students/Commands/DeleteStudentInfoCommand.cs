namespace SchoolManagementSystem.Application.School.Students.Commands;

public record DeleteStudentInfoCommand(Guid id) : IHttpRequest;
