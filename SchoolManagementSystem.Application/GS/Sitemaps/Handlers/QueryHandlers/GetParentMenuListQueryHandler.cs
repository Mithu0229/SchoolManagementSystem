using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetParentMenuListQueryHandler : IHttpRequestHandler<GetParentMenuListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetParentMenuListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(GetParentMenuListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = (await _unitOfWork.DapperCommandQuery.GetDataListAsync<object>
                ("SELECT * FROM dbo.vGetParentMenus ORDER BY OrderSequence;", null, System.Data.CommandType.Text)).ToList();            
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<SitemapResponse>>(StatusCodes.Status500InternalServerError);
        }
    }
}
