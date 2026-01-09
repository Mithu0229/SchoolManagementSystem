using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Sitemaps.Commands;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.CommandHandlers;

public class DeleteSitemapCommandHandler : IHttpRequestHandler<DeleteSitemapCommand>
{
    private IUnitOfWork _unitOfWork;

    public DeleteSitemapCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(DeleteSitemapCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }
            var permissionList = await _unitOfWork.RoleMenuRepository.GetAllNoneDeleted().Where(x => x.SitemapId == request.id).ToListAsync();
            if (permissionList.Count > 0)
            {
                foreach (var permission in permissionList) 
                {
                    await _unitOfWork.RoleMenuRepository.InstantDelete(permission);
                }
            }
            var sitemap = await _unitOfWork.SitemapRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (sitemap is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }
            var result = await _unitOfWork.SitemapRepository.InstantDelete(sitemap);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError);
        }
    }
}
