namespace SchoolManagementSystem.Application.School.Sections.Queries;

public record GetSectionByIdQuery(Guid Id) : IHttpRequest;
