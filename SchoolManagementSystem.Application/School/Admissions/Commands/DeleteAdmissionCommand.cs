namespace SchoolManagementSystem.Application.School.Admissions.Commands;

public record DeleteAdmissionCommand(Guid id) : IHttpRequest;
