namespace SchoolManagementSystem.Application.GS.Roles.Queries;
public record GetRoleByTenantQuery(Guid? id) : IHttpRequest;