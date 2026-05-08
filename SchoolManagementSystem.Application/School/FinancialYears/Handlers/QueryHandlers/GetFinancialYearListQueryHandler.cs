using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FinancialYears.Models;
using SchoolManagementSystem.Application.School.FinancialYears.Queries;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.QueryHandlers;

public class GetFinancialYearListQueryHandler : IHttpRequestHandler<GetFinancialYearListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFinancialYearListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFinancialYearListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.FinancialYearRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.FinYearName.ToLower().Contains(search)
                    || (x.Remarks != null && x.Remarks.ToLower().Contains(search)));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.Select(x => new FinancialYearResponse
            {
                Id = x.Id,
                FinYearName = x.FinYearName,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                FinCode = x.FinCode,
                Remarks = x.Remarks,
                IsCurrent = x.IsCurrent,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);

            var response = new PagedResult<FinancialYearResponse>
            {
                Items = items,
                TotalRecord = totalRecord,
                Page = pagedRequest.Page,
                PageSize = pagedRequest.PageSize
            };
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<FinancialYearResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
