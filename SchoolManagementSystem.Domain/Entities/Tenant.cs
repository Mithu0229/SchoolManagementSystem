namespace SchoolManagementSystem.Domain.Entities;
public class Tenant : AuditableEntity
{
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
    public virtual ICollection<User> TenantUserList { get; set; } = new List<User>();
    public virtual ICollection<Role> TenantRoleList { get; set; } = new List<Role>();
}