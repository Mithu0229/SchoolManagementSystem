using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetAllSitemapQueryHandler : IHttpRequestHandler<GetAllSitemapQuery>
{
    private IPagedService _pagedService;

    public GetAllSitemapQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }

    public async Task<IResult> Handle(GetAllSitemapQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<SitemapResponse>("dbo.sp_GetSitemapList", "dbo.sp_GetSitemapCount", request.MenuPaged, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<SitemapResponse>>(StatusCodes.Status500InternalServerError);
        }
    }
}