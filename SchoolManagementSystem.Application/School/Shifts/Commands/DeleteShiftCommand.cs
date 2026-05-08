namespace SchoolManagementSystem.Application.School.Shifts.Commands;

public record DeleteShiftCommand(Guid id) : IHttpRequest;
