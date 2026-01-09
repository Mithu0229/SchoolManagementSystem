using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Users.Commands;
using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.CommandHandlers;
public class UserLoginCommandHandler : IHttpRequestHandler<UserLoginCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoginService _loginService;
    public UserLoginCommandHandler(IUnitOfWork unitOfWork, ILoginService loginService)
    {
        _unitOfWork = unitOfWork;
        _loginService = loginService;
    }
    public async Task<IResult> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.UserRepository
                .GetAllNoneDeleted(false,true)
                .Where(u => u.Email == request.LoginUser.Email)
                .FirstOrDefaultAsync(cancellationToken);

            //if (user == null)
            //    return Result.Fail<LoginUserResponse>(StatusCodes.Status400BadRequest, "Wrong Email or Password.");
            //else
            //{
            //    if (await IsPasswordValid(request.LoginUser.Password, user.Id))
            //    {
            //        return Result.Success(user);
            //    }
            //}
            var loginResult = await _loginService.LoginSuccessAsync(user, request.LoginUser.RememberMe, cancellationToken);
            return Result.Success(loginResult);

            //return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail<UserResponse>(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }

    private async Task<bool> IsPasswordValid(string password, Guid? userId = null)
    {
        var user = await _unitOfWork.UserRepository
            .GetAllNoneDeleted(false, true)
            .Where(x => x.Id == userId && x.IsActive)
            .FirstOrDefaultAsync();
        if (BCrypt.Net.BCrypt.Verify(password, user!.Password))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}

