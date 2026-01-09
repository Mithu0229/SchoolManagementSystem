using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Commands;
public class ForgotPasswordCommand : IHttpRequest
{
    public ForgotPasswordRequest ForgotPassword { get; set; }
}

