namespace SchoolManagementSystem.Application.School.AcademicClasses.Queries;

public record GetAcademicClassByIdQuery(Guid Id) : IHttpRequest;
