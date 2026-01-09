using SchoolManagementSystem.Application.GS.Sitemaps.Commands;
using SchoolManagementSystem.Application.GS.Sitemaps.Models;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.CommandHandlers;

public class UpdateSitemapCommandHandler : IHttpRequestHandler<UpdateSitemapCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateSitemapCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateSitemapCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<SitemapResponse>(StatusCodes.Status406NotAcceptable);
            }
            if (request.Sitemap.Id != Guid.Empty)
            {
                var sitemap = await _unitOfWork.SitemapRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Sitemap.Id);
                sitemap.Name = request.Sitemap.Name;
                sitemap.PageUrl = request.Sitemap.PageUrl;
                sitemap.SortingOrder = request.Sitemap.SortingOrder;
                sitemap.ParentId = request.Sitemap.ParentId;
                sitemap.IsFeature = request.Sitemap.IsFeature;
                sitemap.IsActive = request.Sitemap.IsActive;
                sitemap.IsSidebarmenu = request.Sitemap.IsSidebarmenu;
                sitemap.FavIcon = request.Sitemap.FavIcon;
                sitemap.MenuType = request.Sitemap.MenuType;
                sitemap.MenuAccessType = request.Sitemap.MenuAccessType;
                await _unitOfWork.SitemapRepository.UpdateAsync(sitemap);
                var result = await _unitOfWork.CommitAsync();
                var response = sitemap.Adapt<SitemapResponse>();
                var parent = await _unitOfWork.SitemapRepository.GetSingleNoneDeletedAsync(x => x.ParentId == sitemap.Id);
                //response.ParentName = parent is null ? string.Empty : parent.Name;
                return Result.Success(response, "Sitemap " + AlertMessage.UpdateMessage);
            }
            return Result.Fail<SitemapResponse>(StatusCodes.Status406NotAcceptable);
        }
        catch (Exception ex)
        {
            // LogHelpers.Error(ex);
            return Result.Fail<SitemapResponse>(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}
