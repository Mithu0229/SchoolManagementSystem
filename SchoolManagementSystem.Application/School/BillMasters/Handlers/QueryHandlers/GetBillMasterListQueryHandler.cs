using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetBillMasterListQueryHandler : IHttpRequestHandler<GetBillMasterListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBillMasterListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetBillMasterListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false,true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => (x.Admission != null && x.Admission.Student.StdCID.ToLower().Contains(search)) || x.BillMonth.ToString().Contains(search) || x.BillYear.ToString().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            query = query.OrderByDescending(x => x.CreatedDate);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            
            
            var items = await query.Select(x => new BillMasterResponse
            {
                Id = x.Id,
                AdmissionId = x.AdmissionId,
                StdCID = x.Admission.Student.StdCID,
                AdmissionRollNo = x.Admission != null ? x.Admission.RollNo : null,
                BillMonth = x.BillMonth,
                BillYear = x.BillYear,
                TotalAmount = x.TotalAmount,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<BillMasterResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<BillMasterResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
