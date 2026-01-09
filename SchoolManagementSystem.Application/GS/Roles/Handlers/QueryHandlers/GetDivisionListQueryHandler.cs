using SchoolManagementSystem.Application.GS.Roles.Queries;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.QueryHandlers;
public class GetRoleListQueryHandler : IHttpRequestHandler<GetRoleListQuery>
{
    private IPagedService _pagedService;
    public GetRoleListQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<RoleResponse>("dbo.sp_get_Roles", "dbo.sp_get_Roles_count", request.PagedRequest, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<RoleResponse>>(StatusCodes.Status500InternalServerError);
        }
    }

}

