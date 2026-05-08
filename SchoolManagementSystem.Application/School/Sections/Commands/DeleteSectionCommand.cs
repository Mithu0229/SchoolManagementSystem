namespace SchoolManagementSystem.Application.School.Sections.Commands;

public record DeleteSectionCommand(Guid id) : IHttpRequest;
