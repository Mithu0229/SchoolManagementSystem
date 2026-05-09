using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeCollections.Models;
using SchoolManagementSystem.Application.School.FeeCollections.Queries;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.QueryHandlers;

public class GetFeeCollectionListQueryHandler : IHttpRequestHandler<GetFeeCollectionListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeCollectionListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeCollectionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.FeeCollectionRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => (x.MemoNo != null && x.MemoNo.ToLower().Contains(search)) || (x.PaymentMode != null && x.PaymentMode.ToLower().Contains(search)));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new FeeCollectionResponse
            {
                Id = x.Id,
                CollectionDate = x.CollectionDate,
                MemoNo = x.MemoNo,
                StudentId = x.StudentId,
                AdmissionId = x.AdmissionId,
                BranchId = x.BranchId,
                FinancialYearId = x.FinancialYearId,
                TotalAmount = x.TotalAmount,
                DiscountAmount = x.DiscountAmount,
                PaidAmount = x.PaidAmount,
                DueAmount = x.DueAmount,
                PaymentMode = x.PaymentMode,
                Remarks = x.Remarks,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<FeeCollectionResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<FeeCollectionResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
