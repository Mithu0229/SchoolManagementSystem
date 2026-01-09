
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.GS.Roles.Commands;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.CommandHandlers;
public class InsertRoleCommandHandler : IHttpRequestHandler<InsertRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
   
    public InsertRoleCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(InsertRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                return Result.Fail<RoleResponse>(StatusCodes.Status406NotAcceptable);

            
            var roleModel = request?.RolePermission?.Role;

            // Determine admin type
            //var isSuperAdmin = _currentUserService.IsSuperAdmin;

            // Role name duplicate check (per tenant or global)
            bool roleExists = await _unitOfWork.RoleRepository
                .GetAll()
                .AnyAsync(x => x.RoleName.ToLower() == roleModel!.RoleName.ToLower() && x.TenantId == roleModel.TenantId);

            if (roleExists)
                return Result.Fail<RoleResponse>(StatusCodes.Status403Forbidden, "Role name already exists");

            // New role
            if (roleModel.Id == Guid.Empty)
            {
                roleModel.Id = Guid.NewGuid();

                //if (_currentUserService.Roles!.Contains(Guid.Empty))//_fieldValue.SuperAdmin_Role_ID
                //{
                //    // SuperAdmin creates global role
                //    roleModel.TenantId = null;
                //    roleModel.RoleType = (int)RoleTypes.Systems;
                //}
                //else
                //{
                    // TenantAdmin must have a TenantId
                    if (_currentUserService.TenantId == null || _currentUserService.TenantId == Guid.Empty)
                        return Result.Fail<RoleResponse>(StatusCodes.Status400BadRequest, "TenantId is required.");

                    roleModel.TenantId = _currentUserService.TenantId;
                    roleModel.RoleType = (int)RoleTypes.Tenant;
               // }

                var role = roleModel.Adapt<Role>();
                await _unitOfWork.RoleRepository.AddAsync(role);
            }
            else
            {
                var existingRole = await _unitOfWork.RoleRepository.GetSingleNoneDeletedAsync(x => x.Id == roleModel.Id);
                if (existingRole == null)
                    return Result.Fail<RoleResponse>(StatusCodes.Status404NotFound, "Role not found");

                await _unitOfWork.RoleRepository.UpdateAsync(existingRole);
            }

            // Remove existing role-menu entries
            var existingMenus = await _unitOfWork.RoleMenuRepository
                .GetAllNoneDeleted()
                .Where(x => x.RoleId == roleModel.Id)
                .ToListAsync();

            foreach (var menu in existingMenus)
            {
                await _unitOfWork.RoleMenuRepository.DeleteAsync(menu);
            }

            // Insert new role-menu entries
            var roleMenus = request.RolePermission.RoleMenus.Adapt<List<RoleMenu>>();
            foreach (var menu in roleMenus)
            {
                menu.RoleId = roleModel.Id;
                menu.CreatedById = request.UserId;
                menu.CreatedDate = DateTime.UtcNow;
                menu.IsActive = roleModel.IsActive;
              //  if (!_currentUserService.Roles!.Contains(Guid.Empty))//_fieldValue.SuperAdmin_Role_ID
                    menu.TenantId = _currentUserService.TenantId;
                await _unitOfWork.RoleMenuRepository.AddAsync(menu);
            }
            await _unitOfWork.CommitAsync();
            var result = await _unitOfWork.RoleRepository.GetSingleNoneDeletedAsync(x => x.Id == roleModel.Id);
            var response = result.Adapt<RoleResponse>();
            return Result.Success(response, "Role " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Error while inserting role");
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.InnerException?.Message ?? ex.Message);
        }
    }
}



