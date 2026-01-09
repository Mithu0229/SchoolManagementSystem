using SchoolManagementSystem.Application.GS.Tenants.Models;

namespace SchoolManagementSystem.Application.GS.Tenants.Commands;

public class InsertTenantCommand : IHttpRequest
{
    public TenantRequest? Tenant { get; set; }
}
