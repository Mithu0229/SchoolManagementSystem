namespace SchoolManagementSystem.Application.School.AcademicSessions.Queries;

public record GetAcademicSessionByIdQuery(Guid Id) : IHttpRequest;
