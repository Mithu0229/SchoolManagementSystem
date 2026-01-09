using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Sitemaps.Commands;
using SchoolManagementSystem.Application.GS.Sitemaps.Models;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.CommandHandlers;

public class InsertSitemapCommandHandler : IHttpRequestHandler<InsertSitemapCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertSitemapCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(InsertSitemapCommand request, CancellationToken cancellationToken)
    {
        try
        {

            if (request is null)
            {
                return Result.Fail<SitemapResponse>(StatusCodes.Status406NotAcceptable);
            }
            var menuName = await _unitOfWork.SitemapRepository
            .GetAllNoneDeleted().Where(x => x.Name.ToLower() == request.Sitemap.Name.ToLower())
            .FirstOrDefaultAsync();
            if (menuName is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Name already exists!");
            }
            var pageUrl = await _unitOfWork.SitemapRepository
            .GetAllNoneDeleted().Where(x => x.PageUrl.ToLower() == request.Sitemap.PageUrl.ToLower())
            .FirstOrDefaultAsync();
            if (pageUrl is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "PageUrl already exists!");
            }
            var sitemap = request.Sitemap.Adapt<Sitemap>();
            await _unitOfWork.SitemapRepository.AddAsync(sitemap);
            var result = await _unitOfWork.CommitAsync();
            var response = sitemap.Adapt<SitemapResponse>();
            return Result.Success(response, "Sitemap " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<SitemapResponse>(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}

