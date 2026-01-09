using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Users.Commands;
using SchoolManagementSystem.Application.GS.Users.FluentValidations;
using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.CommandHandlers;
public class InsertUserCommandHandler : IHttpRequestHandler<InsertUserCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public InsertUserCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<IResult> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        var validator = await new InsertUserCommandValidator().ValidateAsync(request, cancellationToken);
        if (validator.IsValid is false)
        {
            return Result.Fail<string>(StatusCodes.Status400BadRequest, validator.Errors);
        }

        try
        {
            if (request is null)
            {
                return Result.Fail<UserResponse>(StatusCodes.Status406NotAcceptable);
            }
            var userDbObj = await _unitOfWork.UserRepository.GetAllNoneDeleted().FirstOrDefaultAsync(x => x.Email.Trim() == request.User!.Email!.Trim() && x.TenantId == _currentUserService.TenantId, cancellationToken);
            if (userDbObj != null)
            {
                return Result.Fail<UserResponse>(StatusCodes.Status406NotAcceptable, "User with this email already exists.");
            }

            var entity = request.User.Adapt<User>();
            entity.TenantId = _currentUserService.TenantId;
            entity.Password = BCrypt.Net.BCrypt.HashPassword(request.User!.Password);
            await _unitOfWork.UserRepository.AddAsync(entity);
            var result = await _unitOfWork.CommitAsync();
            var response = entity.Adapt<UserResponse>();
            return Result.Success(response, "User " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<UserResponse>(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}



