namespace SchoolManagementSystem.Application.GS.Tenants.Queries;

public record GetTenantByIdQuery(Guid id):IHttpRequest;