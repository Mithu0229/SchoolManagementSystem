using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.GS.Roles.Queries;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.QueryHandlers;
public class GetRoleByTenantQueryHandler : IHttpRequestHandler<GetRoleByTenantQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetRoleByTenantQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }
    public async Task<IResult> Handle(GetRoleByTenantQuery request, CancellationToken cancellationToken)
    {
        try
        {
            //if (request.id == Guid.Empty)
            //{
            //    var result = await _unitOfWork.RoleRepository.GetAllNoneDeleted(false, true)
            //        .Where(x => x.IsActive &&
            //        x.TenantId == null &&
            //        x.Id != _fieldValue.SuperAdmin_Role_ID
            //        )
            //        .Select(s => new
            //        {
            //            s.Id,
            //            s.RoleName,
            //            s.TenantId,
            //        }).OrderBy(o => o.RoleName).ToListAsync(cancellationToken);
            //    if (result == null || !result.Any())
            //        return Result.Fail(StatusCodes.Status404NotFound, "No roles found for the specified tenant.");
            //    return Result.Success(result);
            //}
            //else
            //{
                var result = await _unitOfWork.RoleRepository.GetAllNoneDeleted().Where(x => x.IsActive).Select(s => new
                {
                    s.Id,
                    s.RoleName,
                    s.TenantId,
                }).OrderBy(o => o.RoleName).ToListAsync(cancellationToken);
                if (result == null || !result.Any())
                    return Result.Fail(StatusCodes.Status404NotFound, "No roles found for the specified tenant.");
                return Result.Success(result);
            //}
        }
        catch (Exception ex)
        {
            return Result.Fail<RoleResponse>(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}
