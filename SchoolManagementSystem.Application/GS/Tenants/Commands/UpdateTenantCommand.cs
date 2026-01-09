using SchoolManagementSystem.Application.GS.Tenants.Models;

namespace SchoolManagementSystem.Application.GS.Tenants.Commands;

public class UpdateTenantCommand : IHttpRequest
{
    public TenantRequest? Tenant { get; set; }
}
