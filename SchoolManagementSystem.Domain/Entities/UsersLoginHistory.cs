namespace SchoolManagementSystem.Domain.Entities;
public class UsersLoginHistory : TenantEntity
{
    public string? IP { get; set; }
    public string? MAC { get; set; }
    public string? NetworkType { get; set; }
    public string? OperatingSystem { get; set; }
}
