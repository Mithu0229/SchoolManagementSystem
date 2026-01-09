namespace SchoolManagementSystem.Application.GS.Divisions.Commands;

public record DeleteDivisionCommand(Guid id) : IHttpRequest;
