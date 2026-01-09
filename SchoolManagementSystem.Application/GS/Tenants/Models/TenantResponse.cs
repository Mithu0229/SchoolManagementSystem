using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Tenants.Models;

public class TenantResponse
{
    public Guid Id { get; set; }
    public required string TenantName { get; set; }
    public string? BinNo { get; set; }
    public required string TenantEmail { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Domain { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostCode { get; set; }
    public string? Reason { get; set; }
    public bool IsActive { get; set; }
    public List<UserResponse> TenantUserList { get; set; } = new List<UserResponse>();

}
