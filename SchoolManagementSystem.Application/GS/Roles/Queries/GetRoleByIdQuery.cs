namespace SchoolManagementSystem.Application.GS.Roles.Queries;
public record GetRoleByIdQuery(Guid Id) : IHttpRequest;