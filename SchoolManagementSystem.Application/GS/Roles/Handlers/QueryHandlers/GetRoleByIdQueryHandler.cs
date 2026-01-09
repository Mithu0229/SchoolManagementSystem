using Microsoft.IdentityModel.Logging;
using SchoolManagementSystem.Application.GS.Roles.Queries;
using SchoolManagementSystem.Domain.Entities;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.QueryHandlers;
public class GetRoleByIdQueryHandler : IHttpRequestHandler<GetRoleByIdQuery>
{
    private IUnitOfWork _unitOfWork;

    public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var parameters = new Dictionary<string, object>
            {
                { "roleId", request.Id! }
            };

            // Fix: Correct the type conversion issue by ensuring the result is properly cast to the expected type.
            //var roleMenus = (await _unitOfWork.DapperCommandQuery.GetDataListAsync<RoleMenuResponse>(
            //    "SELECT * FROM fngetrolewisepermission(@roleId);", parameters, System.Data.CommandType.Text)).ToList();

            var roleMenus = (await _unitOfWork.DapperCommandQuery.GetDataListAsync<RoleMenuResponse>(
    "dbo.sp_GetRoleWisePermission",
    parameters,
    System.Data.CommandType.StoredProcedure)).ToList();

            return Result.Success(roleMenus);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<RoleMenuResponse>(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}