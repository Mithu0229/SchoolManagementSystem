using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Application.Common;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolManagementSystem.Infrastructure.Persistence.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = Guid.TryParse(userIdClaim, out var id) ? id : (Guid?)null;
            // Add a log or breakpoint here
            //Console.WriteLine($"CurrentUserService.UserId: {result}");
            return result;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
            var result = Guid.TryParse(tenantIdClaim, out var tid) ? tid : (Guid?)null;
            //Console.WriteLine($"CurrentUserService.TenantId: {result}");
            return result;
        }
    }

    public Guid? RoleId
    {
        get
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("roleid")?.Value;
            var result = Guid.TryParse(roleClaim, out var rid) ? rid : (Guid?)null;
            //Console.WriteLine($"CurrentUserService.RoleId: {result}");
            return result;
        }
    }

    public string? IpAddress
    {
        get
        {
            var result = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";
            //Console.WriteLine($"CurrentUserService.RoleId: {result}");
            return result;
        }
    }

    public List<Guid>? Roles
    {
        get
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(roleClaim))
                return new List<Guid>();

            try
            {
                var roles = JsonSerializer.Deserialize<List<UserRoles>>(roleClaim);
                return roles?.Select(r => r.RoleId).ToList() ?? new List<Guid>();
            }
            catch
            {
                // Optional: log or handle bad claim format
                return new List<Guid>();
            }
        }
    }

    public class UserRoles
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

}
