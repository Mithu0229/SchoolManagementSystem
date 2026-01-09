using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Application.GS.Users.Models;
public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
