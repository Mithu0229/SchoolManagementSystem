namespace SchoolManagementSystem.Application.School.Admissions.Queries;

public record GetAdmissionByIdQuery(Guid Id) : IHttpRequest;
