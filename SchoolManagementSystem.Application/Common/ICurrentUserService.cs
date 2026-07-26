namespace SchoolManagementSystem.Application.Common;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    List<Guid>? Roles { get; }
    Guid? RoleId { get; }
    string? IpAddress { get; }
}

public interface ISmsService
{
    Task<bool> SendSmsAsync(string mobile, string message);
}
