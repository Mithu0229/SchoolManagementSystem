namespace SchoolManagementSystem.Application.School.StudentGroups.Commands;

public record DeleteStudentGroupCommand(Guid id) : IHttpRequest;
