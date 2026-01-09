using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Commands;
public class UserLoginCommand : IHttpRequest
{
    public LoginUserRequest LoginUser { get; set; }

}
