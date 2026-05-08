namespace SchoolManagementSystem.Application.School.AcademicSessions.Commands;

public record DeleteAcademicSessionCommand(Guid id) : IHttpRequest;
