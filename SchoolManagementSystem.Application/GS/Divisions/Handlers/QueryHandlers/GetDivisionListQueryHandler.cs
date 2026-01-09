using SchoolManagementSystem.Application.GS.Divisions.Models;
using SchoolManagementSystem.Application.GS.Divisions.Queries;

namespace SchoolManagementSystem.Application.GS.Divisions.Handlers.QueryHandlers;
public class GetDivisionListQueryHandler : IHttpRequestHandler<GetDivisionListQuery>
{
    private IPagedService _pagedService;
    public GetDivisionListQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetDivisionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<DivisionResponse>("dbo.sp_get_divisions", "dbo.sp_get_divisions_count", request.PagedRequest, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<DivisionResponse>>(StatusCodes.Status500InternalServerError);
        }
    }

}

