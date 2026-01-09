namespace SchoolManagementSystem.Application.GS.Tenants.Queries;

public record GetAllTenantsQuery(int pg, int sl, string? sv) : IHttpRequest;