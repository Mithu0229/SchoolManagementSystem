namespace SchoolManagementSystem.Application.GS.Tenants.Queries;

public record GetTenantListQuery() : IHttpRequest
{
    public PagedRequest? PagedRequest { get; set; }
}
