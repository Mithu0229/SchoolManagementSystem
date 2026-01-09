using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetFeatureListQueryHandler : IHttpRequestHandler<GetFeatureListQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetFeatureListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFeatureListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.SitemapRepository.GetAllNoneDeleted().Where(x => x.IsFeature == true && x.IsActive)
                .Select(s => new
                {
                    Id= s.Id,
                    Name= s.Name,
                    SortingOrder=s.SortingOrder,
                }).OrderBy(x=>x.SortingOrder).ToListAsync();
            return Result.Success(entity);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
