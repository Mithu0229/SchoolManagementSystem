using Microsoft.IdentityModel.Logging;
using SchoolManagementSystem.Application.GS.Roles.Queries;
using SchoolManagementSystem.Application.GS.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.QueryHandlers;

public class GetRolesPagesQueryHandler : IHttpRequestHandler<GetRolesPagesQuery>
{
    private IPagedService _pagedService;
    public GetRolesPagesQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetRolesPagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<RoleResponse>("dbo.sp_GetRoles", "dbo.sp_GetRolesCount", request.RolesPaged!, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<RoleResponse>>(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
        }
    }
}

