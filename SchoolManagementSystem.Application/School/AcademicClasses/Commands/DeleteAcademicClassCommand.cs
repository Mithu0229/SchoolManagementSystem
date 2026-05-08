namespace SchoolManagementSystem.Application.School.AcademicClasses.Commands;

public record DeleteAcademicClassCommand(Guid id) : IHttpRequest;
