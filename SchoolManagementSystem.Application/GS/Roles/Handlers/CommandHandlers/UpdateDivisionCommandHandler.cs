using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Roles.Commands;
using SchoolManagementSystem.Application.GS.Roles.FluentValidations;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.CommandHandlers;

public class UpdateRoleCommandHandler : IHttpRequestHandler<UpdateRoleCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;

    }

    public async Task<IResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var validator = await new UpdateRoleCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validator.IsValid)
                return Result.Fail<string>(StatusCodes.Status400BadRequest, validator.Errors);

            if (request is null || request.RolePermission?.Role?.Id == Guid.Empty)
                return Result.Fail<RoleResponse>(StatusCodes.Status406NotAcceptable);


            var roleDto = request.RolePermission!.Role;

            // Check duplicate name
            var isDuplicate = await _unitOfWork.RoleRepository
                .GetAll()
                .AnyAsync(x => x.RoleName.ToLower() == roleDto.RoleName.ToLower()
                            && x.TenantId == roleDto.TenantId
                            && x.Id != roleDto.Id);
            if (isDuplicate)
                return Result.Fail<RoleResponse>(StatusCodes.Status403Forbidden, "Role name already exists");

            // Fetch and update Role
            var role = await _unitOfWork.RoleRepository.GetAllNoneDeleted(false, true).FirstOrDefaultAsync(x => x.Id == roleDto.Id);
            if (role == null)
                return Result.Fail<RoleResponse>(StatusCodes.Status404NotFound, "Role not found");

            role.RoleName = roleDto.RoleName!;
            role.Description = roleDto.Description!;
            role.IsActive = roleDto.IsActive;
            role.ModifiedById = request.UserId;
            role.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.RoleRepository.UpdateAsync(role);

            // Replace RoleMenus
            var menus = request.RolePermission.RoleMenus?
                .Where(mn => mn.CanView || mn.CanAdd || mn.CanEdit || mn.CanDelete || mn.CanPreview || mn.CanPrint || mn.CanExport)
                .Adapt<List<RoleMenu>>() ?? new();

            foreach (var menu in menus)
            {
                menu.RoleId = role.Id;
                menu.CreatedById = request.UserId;
                menu.CreatedDate = DateTime.UtcNow;
                menu.IsActive = true;
            }

            await _unitOfWork.RoleMenuRepository.ReplaceManyAsync<RoleMenu>(
                x => x.RoleId == role.Id,
                menus,
                useSoftDelete: true);

            await _unitOfWork.CommitAsync();
            var updated = await _unitOfWork.RoleRepository.GetSingleNoneDeletedAsync(x => x.Id == role.Id);
            return Result.Success(updated.Adapt<RoleResponse>(), "Role " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            // LogHelpers.Error(ex);
            return Result.Fail<RoleResponse>(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
        }

    }
}