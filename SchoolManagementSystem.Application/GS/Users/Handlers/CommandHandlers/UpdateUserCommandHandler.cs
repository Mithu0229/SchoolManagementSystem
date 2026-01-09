using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Users.Commands;
using SchoolManagementSystem.Application.GS.Users.FluentValidations;
using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.CommandHandlers;
public class UpdateUserCommandHandler : IHttpRequestHandler<UpdateUserCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork) 
    { 
        _unitOfWork = unitOfWork;
        
    }
    public async Task<IResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = await new UpdateUserCommandValidator().ValidateAsync(request, cancellationToken);

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

            var user = await _unitOfWork.UserRepository.GetAllNoneDeleted()
                .FirstOrDefaultAsync(u => u.Id == request.User.Id);

            if (user == null)
            {
                return Result.Fail<UserResponse>(StatusCodes.Status404NotFound, "User not found.");
            }
            if (user.Email != request.User.Email)
            {
                var emailExists = await _unitOfWork.UserRepository.GetAllNoneDeleted()
                    .AnyAsync(u => u.Email == request.User.Email && u.Id != request.User.Id);

                if (emailExists)
                {
                    return Result.Fail<UserResponse>(StatusCodes.Status400BadRequest, "Email already in use by another user.");
                }
            }
            if (request.User.Id != Guid.Empty)
            {
               // var userDbObj = await _unitOfWork.UserRepository.GetSingleNoneDeletedAsync(x => x.Id == request.User.Id);
                if (user != null)
                {
                    user.FirstName = request.User.FirstName;
                    user.LastName = request.User.LastName;
                    user.Email = request.User.Email;
                    user.PhoneNumber = request.User.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(request.User.Password))
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
                    }
                    user.Address = request.User.Address;
                    user.UserType = request.User.UserType;
                    user.IsActive = true;

                    await UpdateUserRole(request);
                    await _unitOfWork.UserRepository.UpdateAsync(user);
                    await _unitOfWork.CommitAsync();
                    var result = await _unitOfWork.UserRepository.GetSingleNoneDeletedAsync(x => x.Id == request.User.Id);
                    var response = result.Adapt<UserResponse>();
                    return Result.Success(response);
                }
            }
            return Result.Fail<UserResponse>(StatusCodes.Status406NotAcceptable);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<UserResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private async Task UpdateUserRole(UpdateUserCommand request)
    {
        if(request.User.UserRoleList != null)
        {
            if (request.User.UserRoleList!.Count() > 0)
            {
                var newRoleIds = request.User.UserRoleList!.Select(ur => ur.RoleId).ToHashSet();
                var existingUserRoles = _unitOfWork.UserRoleRepository
                    .GetAllNoneDeleted()
                    .Where(x => x.UserId == request.User.Id)
                    .ToList();

                var existingRoleIds = existingUserRoles.Select(ur => ur.RoleId).ToHashSet();
                var rolesToAdd = newRoleIds.Except(existingRoleIds).ToList();
                var rolesToRemove = existingRoleIds.Except(newRoleIds).ToList();

                if (rolesToRemove.Any())
                {
                    var rolesToDelete = existingUserRoles
                        .Where(ur => rolesToRemove.Contains(ur.RoleId))
                        .ToList();

                    foreach (var role in rolesToDelete)
                    {
                        role.IsDeleted = true;
                        role.ModifiedById = request.User.Id;
                    }
                    await _unitOfWork.UserRoleRepository.UpdateRangeAsync(rolesToDelete);
                }
                if (rolesToAdd.Any())
                {
                    var roles = await _unitOfWork.RoleRepository.GetAllNoneDeleted()
                        .Where(r => rolesToAdd.Contains(r.Id))
                        .ToListAsync();

                    var newUserRoles = roles.Select(role => new UserRole
                    {
                        UserId = request.User.Id,
                        RoleId = role.Id,
                        IsActive = true
                    }).ToList();

                    await _unitOfWork.UserRoleRepository.AddRangeAsync(newUserRoles);
                }
            }
            else
            {
                var existingRoles = await _unitOfWork.UserRoleRepository.GetAllNoneDeleted().Where(x => x.UserId == request.User.Id).ToListAsync();
                foreach (var role in existingRoles)
                {
                    await _unitOfWork.UserRoleRepository.InstantDelete(role);
                }

            }
        }
       
    }

}
