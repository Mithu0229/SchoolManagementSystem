using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetStudentBillHistorySummaryQueryHandler : IHttpRequestHandler<GetStudentBillHistorySummaryQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentBillHistorySummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentBillHistorySummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();

            var cashQuery = _unitOfWork.CashBookRepository.GetAllNoneDeleted(false, true)
                .Where(c => c.Debit > 0);

            var bankQuery = _unitOfWork.BankBookRepository.GetAllNoneDeleted(false, true)
                .Where(b => b.Debit > 0);

            var billsQuery = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true);

            var query = billsQuery.Select(b => new
            {
                b.Id,
                StudentId = b.Admission != null ? (Guid?)b.Admission.StudentId : null,
                StudentName = b.Admission != null && b.Admission.Student != null ? (b.Admission.Student.FullName ?? "") : "",
                StdCID = b.Admission != null && b.Admission.Student != null ? (b.Admission.Student.StdCID ?? "") : "",
                b.BillMonth,
                b.BillYear,
                b.VoucherNo,
                b.TotalAmount,
                PaidCash = cashQuery.Where(c => c.BillMasterId == b.Id).Sum(c => (decimal?)c.Debit) ?? 0m,
                PaidBkash = bankQuery.Where(bk => bk.BillMasterId == b.Id && bk.TransactionType == TransactionType.Bkash).Sum(bk => (decimal?)bk.Debit) ?? 0m,
                PaidBank = bankQuery.Where(bk => bk.BillMasterId == b.Id && bk.TransactionType == TransactionType.Bank).Sum(bk => (decimal?)bk.Debit) ?? 0m
            });

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x =>
                    x.StudentName.ToLower().Contains(search) ||
                    x.StdCID.ToLower().Contains(search) ||
                    x.BillMonth.ToString().Contains(search) ||
                    x.BillYear.ToString().Contains(search) ||
                    (x.VoucherNo != null && x.VoucherNo.ToLower().Contains(search))
                );
            }

            if (pagedRequest.Filters != null && pagedRequest.Filters.Count > 0)
            {
                foreach (var filter in pagedRequest.Filters)
                {
                    if (filter.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString())) continue;
                    var valStr = filter.Value.ToString()!.Trim();

                    switch (filter.Field.ToLower())
                    {
                        case "stdcid":
                            query = query.Where(x => x.StdCID.ToLower() == valStr.ToLower());
                            break;
                        case "studentid":
                            if (Guid.TryParse(valStr, out var sGuid))
                                query = query.Where(x => x.StudentId == sGuid);
                            break;
                        case "billmonth":
                        case "month":
                            if (int.TryParse(valStr, out var mVal))
                                query = query.Where(x => x.BillMonth == mVal);
                            break;
                        case "billyear":
                        case "year":
                            if (int.TryParse(valStr, out var yVal))
                                query = query.Where(x => x.BillYear == yVal);
                            break;
                    }
                }
            }

            var count = await query.CountAsync(cancellationToken);
            var totalAmount = await query.SumAsync(x => x.TotalAmount, cancellationToken);
            var totalCash = await query.SumAsync(x => x.PaidCash, cancellationToken);
            var totalBkash = await query.SumAsync(x => x.PaidBkash, cancellationToken);
            var totalBank = await query.SumAsync(x => x.PaidBank, cancellationToken);
            var totalPaid = totalCash + totalBkash + totalBank;
            var totalDue = Math.Max(0m, totalAmount - totalPaid);

            var summary = new StudentBillHistorySummaryResponse
            {
                TotalBillsCount = count,
                OverallTotalAmount = totalAmount,
                OverallTotalPaidAmount = totalPaid,
                OverallTotalDueAmount = totalDue,
                OverallTotalPaidCashAmount = totalCash,
                OverallTotalPaidBkashAmount = totalBkash
            };

            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentBillHistorySummaryResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
