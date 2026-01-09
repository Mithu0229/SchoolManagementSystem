namespace SchoolManagementSystem.Application.GS.Users.Commands;

public record DeleteUserCommand(Guid id) : IHttpRequest;
