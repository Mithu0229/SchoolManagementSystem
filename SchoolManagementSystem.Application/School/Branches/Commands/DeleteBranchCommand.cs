namespace SchoolManagementSystem.Application.School.Branches.Commands;

public record DeleteBranchCommand(Guid id) : IHttpRequest;
