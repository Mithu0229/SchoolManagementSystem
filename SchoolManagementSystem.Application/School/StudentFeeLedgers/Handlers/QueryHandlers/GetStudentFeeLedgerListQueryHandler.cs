using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Queries;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Handlers.QueryHandlers;

public class GetStudentFeeLedgerListQueryHandler : IHttpRequestHandler<GetStudentFeeLedgerListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentFeeLedgerListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentFeeLedgerListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.StudentFeeLedgerRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.Student.FullName.ToLower().Contains(search) || (x.MemoNo != null && x.MemoNo.ToLower().Contains(search)) || (x.VoucherCode != null && x.VoucherCode.ToLower().Contains(search)));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new StudentFeeLedgerResponse
            {
                Id = x.Id,
                EntryDate = x.EntryDate,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                AdmissionId = x.AdmissionId,
                BranchId = x.BranchId,
                ClassId = x.ClassId,
                FinancialYearId = x.FinancialYearId,
                FinYearName = x.FinancialYear.FinYearName,
                MonthNo = x.MonthNo,
                YearNo = x.YearNo,
                FeeAmount = x.FeeAmount,
                CollectionAmount = x.CollectionAmount,
                DueAmount = x.DueAmount,
                MemoNo = x.MemoNo,
                VoucherCode = x.VoucherCode,
                Remarks = x.Remarks,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<StudentFeeLedgerResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<StudentFeeLedgerResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
