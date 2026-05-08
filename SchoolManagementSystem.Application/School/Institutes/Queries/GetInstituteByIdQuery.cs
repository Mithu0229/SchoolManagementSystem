namespace SchoolManagementSystem.Application.School.Institutes.Queries;

public record GetInstituteByIdQuery(Guid Id) : IHttpRequest;
