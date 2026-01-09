using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Users.Models;

public class UserRequest
{
    public Guid Id { get; set; }
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? Email { get; set; }
    public  string? PhoneNumber { get; set; }
    public  string? Password { get; set; }
    public UserTypes UserType { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }
    public virtual List<UserRoleRequest>? UserRoleList { get; set; }
}
