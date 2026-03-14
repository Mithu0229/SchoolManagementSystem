using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities;

public class User:TenantEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public Guid? StudentId { get; set; }
    public UserTypes UserType { get; set; }
    public string? Address { get; set; }
    public virtual Tenant Tenant { get; set; }
    public virtual ICollection<UserRole> UserRoleList { get; set; }

}
