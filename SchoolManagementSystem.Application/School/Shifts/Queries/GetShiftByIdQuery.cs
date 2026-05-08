namespace SchoolManagementSystem.Application.School.Shifts.Queries;

public record GetShiftByIdQuery(Guid Id) : IHttpRequest;
