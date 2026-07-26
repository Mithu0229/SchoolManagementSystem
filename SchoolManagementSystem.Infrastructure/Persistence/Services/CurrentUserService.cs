using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SmsService(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<bool> SendSmsAsync(string mobile, string message)
    {

        var smsPayload = new
        {
            apikey = _configuration["SmsSettings:ApiKey"],
            secretkey = _configuration["SmsSettings:SecretKey"],
            callerID = _configuration["SmsSettings:CallerID"],
            toUser = "8801755948794",
            messageContent = message
        };

        using var httpClient = new System.Net.Http.HttpClient();
        var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(smsPayload), System.Text.Encoding.UTF8, "application/json");
        // Note: Ensure the endpoint path (e.g. /api/v1/send or /smsapi) matches what Songbird Telecom expects
        var response = await httpClient.PostAsync("http://sms.songbirdtelecom.com:8746/sendtext", content);

        return response.IsSuccessStatusCode;
    }
}
