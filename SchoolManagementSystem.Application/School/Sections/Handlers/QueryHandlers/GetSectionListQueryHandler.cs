using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Sections.Models;
using SchoolManagementSystem.Application.School.Sections.Queries;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.QueryHandlers;

public class GetSectionListQueryHandler : IHttpRequestHandler<GetSectionListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetSectionListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetSectionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.SectionRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.SectionName.ToLower().Contains(search) || (x.Remarks != null && x.Remarks.ToLower().Contains(search)));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new SectionResponse { Id = x.Id, SectionName = x.SectionName, Remarks = x.Remarks, IsActive = x.IsActive }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<SectionResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<SectionResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
