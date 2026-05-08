namespace SchoolManagementSystem.Application.School.Institutes.Commands;

public record DeleteInstituteCommand(Guid id) : IHttpRequest;
