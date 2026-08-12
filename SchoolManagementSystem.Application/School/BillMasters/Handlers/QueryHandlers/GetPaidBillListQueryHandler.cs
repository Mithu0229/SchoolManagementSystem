using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetPaidBillListQueryHandler : IHttpRequestHandler<GetPaidBillListQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPaidBillListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

    public async Task<IResult> Handle(GetPaidBillListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x =>
                    (x.Admission != null && x.Admission.Student != null && (
                        (x.Admission.Student.FullName != null && x.Admission.Student.FullName.ToLower().Contains(search)) ||
                        (x.Admission.Student.StdCID != null && x.Admission.Student.StdCID.ToLower().Contains(search))
                    )) ||
                    x.BillMonth.ToString().Contains(search) ||
                    x.BillYear.ToString().Contains(search) ||
                    x.TransactionType.ToString().ToLower().Contains(search)
                );
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            query = query.OrderByDescending(x => x.CreatedDate);

            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var rawItems = await query.Select(x => new
            {
                x.Id,
                x.AdmissionId,
                StudentName = x.Admission != null && x.Admission.Student != null ? x.Admission.Student.FullName : null,
                StdCID = x.Admission != null && x.Admission.Student != null ? x.Admission.Student.StdCID : null,
                TransactionType = x.TransactionType.ToString(),
                x.BillMonth,
                x.BillYear,
                x.TotalAmount,
                x.IsActive,
                x.CreatedDate
            }).ToListAsync(cancellationToken);

            var monthNames = new[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var items = rawItems.Select(x => new PaidBillResponse
            {
                Id = x.Id,
                AdmissionId = x.AdmissionId,
                StudentName = x.StudentName ?? "N/A",
                StdCID = x.StdCID ?? "N/A",
                TransactionType = x.TransactionType,
                BillMonth = x.BillMonth,
                MonthName = x.BillMonth >= 1 && x.BillMonth <= 12 ? monthNames[x.BillMonth] : x.BillMonth.ToString(),
                BillYear = x.BillYear,
                TotalAmount = x.TotalAmount,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate
            }).ToList();

            return Result.Success(new PagedResult<PaidBillResponse>
            {
                Items = items,
                TotalRecord = totalRecord,
                Page = pagedRequest.Page,
                PageSize = pagedRequest.PageSize
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<PaidBillResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
