using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetMenuListQueryHandler : IHttpRequestHandler<GetMenuListQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetMenuListQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(GetMenuListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string query = _currentUserService.TenantId == null
    ? "SELECT * FROM dbo.vw_leaf_sitemaps WHERE ParentId IS NOT NULL ORDER BY CreatedDate, SortingOrder ASC;"
    : "SELECT * FROM dbo.vw_leaf_sitemaps WHERE ParentId IS NOT NULL AND MenuAccessType IN (2,3) ORDER BY CreatedDate, SortingOrder ASC;";

            var result = await _unitOfWork.DapperCommandQuery.GetDataListAsync<SitemapResponse>(
                query, null, System.Data.CommandType.Text);
            var response = result.Adapt<List<SitemapResponse>>();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<SitemapResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
