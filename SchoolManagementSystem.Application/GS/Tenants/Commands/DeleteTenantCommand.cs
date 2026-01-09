namespace SchoolManagementSystem.Application.GS.Tenants.Commands;

public record DeleteTenantCommand(Guid id):IHttpRequest;
