using SchoolManagementSystem.Application.GS.Users.Models;
using SchoolManagementSystem.Application.GS.Users.Queries;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.QueryHandlers;
public class GetUserListQueryHandler : IHttpRequestHandler<GetUserListQuery>
{
    private IPagedService _pagedService;
    public GetUserListQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<UserResponse>("dbo.sp_get_Users", "dbo.sp_get_Users_count", request.PagedRequest, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<UserResponse>>(StatusCodes.Status500InternalServerError);
        }
    }

}

