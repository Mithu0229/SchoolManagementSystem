using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Users.Models;

public class UserResponse
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public UserTypes UserType { get; set; }
    public string? Address { get; set; }
    public string? UserTypeName { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }
    public virtual List<UserRoleRequest>? UserRoleList { get; set; }

}
