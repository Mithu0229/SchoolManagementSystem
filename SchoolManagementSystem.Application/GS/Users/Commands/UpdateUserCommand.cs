using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Commands;
public class UpdateUserCommand : IHttpRequest
{
    public UserRequest User { get; set; }

}

