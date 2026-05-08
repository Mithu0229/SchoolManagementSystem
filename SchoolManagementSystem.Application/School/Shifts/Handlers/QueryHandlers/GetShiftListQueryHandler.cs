using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Shifts.Models;
using SchoolManagementSystem.Application.School.Shifts.Queries;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.QueryHandlers;

public class GetShiftListQueryHandler : IHttpRequestHandler<GetShiftListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetShiftListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetShiftListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.ShiftRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.ShiftName.ToLower().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new ShiftResponse { Id = x.Id, ShiftName = x.ShiftName, IsActive = x.IsActive }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<ShiftResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<ShiftResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
