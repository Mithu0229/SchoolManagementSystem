using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetSitemapByIdQueryHandler : IHttpRequestHandler<GetSitemapByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetSitemapByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(GetSitemapByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<SitemapResponse>(StatusCodes.Status406NotAcceptable);
            }
            var result = await _unitOfWork.SitemapRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            var response = result.Adapt<SitemapResponse>();
            var parent = await _unitOfWork.SitemapRepository.GetSingleNoneDeletedAsync(x => x.ParentId == result.Id);
            //response.ParentName = parent is null ? string.Empty : parent.Name;

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<SitemapResponse>(StatusCodes.Status500InternalServerError);
        }
    }
}
