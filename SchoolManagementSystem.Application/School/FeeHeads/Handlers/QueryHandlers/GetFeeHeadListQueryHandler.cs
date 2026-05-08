using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeHeads.Models;
using SchoolManagementSystem.Application.School.FeeHeads.Queries;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.QueryHandlers;

public class GetFeeHeadListQueryHandler : IHttpRequestHandler<GetFeeHeadListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeHeadListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeHeadListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.FeeHeadRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.FeeHeadName.ToLower().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new FeeHeadResponse { Id = x.Id, FeeHeadName = x.FeeHeadName, IsMonthly = x.IsMonthly, IsActive = x.IsActive }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<FeeHeadResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<FeeHeadResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
