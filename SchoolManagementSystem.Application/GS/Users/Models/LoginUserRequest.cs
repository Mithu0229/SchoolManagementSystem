namespace SchoolManagementSystem.Application.GS.Users.Models;
public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
