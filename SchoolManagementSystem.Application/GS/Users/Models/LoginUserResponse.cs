using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Users.Models;
public class LoginUserResponse
{
    public Guid? UserId { get; set; }
    public  string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string Email { get; set; }
    public  string PhoneNumber { get; set; }
    public  string Password { get; set; }
    public UserTypes UserType { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public string? UserName { get; set; }
    public string? InitialName { get; set; }
    public string? UserEmail { get; set; }
    public string? UserPhone { get; set; }
    public Guid? GroupId { get; set; }
    public string? GroupName { get; set; }
    public bool? IsFirstLogin { get; set; }
    public DateTime? FirstLoginDate { get; set; }
    public DateTime? PasswordResetDate { get; set; }
    public string? UserPhoto { get; set; }
    public string? Logo { get; set; }
    public Guid? TenantId { get; set; }
    public string? TenantName { get; set; }

}