using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Commands;
public class InsertUserCommand : IHttpRequest
{
    public UserRequest? User { get; set; }

}