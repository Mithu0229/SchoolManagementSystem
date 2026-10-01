using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using SchoolManagementSystem.Domain.Enums;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetStudentBillHistoryListQueryHandler : IHttpRequestHandler<GetStudentBillHistoryListQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentBillHistoryListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentBillHistoryListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();

            var cashQuery = _unitOfWork.CashBookRepository.GetAllNoneDeleted(false, true)
                .Where(c => c.Debit > 0);

            var bankQuery = _unitOfWork.BankBookRepository.GetAllNoneDeleted(false, true)
                .Where(b => b.Debit > 0);

            var billsQuery = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true);

            // Base query with exact data source mapping:
            // Total amount from billmaster table
            // Paid amounts from cashbook and bankbook tables
            var query = billsQuery.Select(b => new
            {
                b.Id,
                b.AdmissionId,
                StudentId = b.Admission != null ? (Guid?)b.Admission.StudentId : null,
                StudentName = b.Admission != null && b.Admission.Student != null ? (b.Admission.Student.FullName ?? "") : "",
                StdCID = b.Admission != null && b.Admission.Student != null ? (b.Admission.Student.StdCID ?? "") : "",
                b.BillMonth,
                b.BillYear,
                b.VoucherNo,
                b.TotalAmount,
                TotalPaidCashAmount = cashQuery.Where(c => c.BillMasterId == b.Id).Sum(c => (decimal?)c.Debit) ?? 0m,
                TotalPaidBkashAmount = bankQuery.Where(bk => bk.BillMasterId == b.Id && bk.TransactionType == TransactionType.Bkash).Sum(bk => (decimal?)bk.Debit) ?? 0m,
                TotalPaidBankAmount = bankQuery.Where(bk => bk.BillMasterId == b.Id && bk.TransactionType == TransactionType.Bank).Sum(bk => (decimal?)bk.Debit) ?? 0m,
                CashTrxDate = cashQuery.Where(c => c.BillMasterId == b.Id).Max(c => (DateTime?)c.TransactionDate),
                BankTrxDate = bankQuery.Where(bk => bk.BillMasterId == b.Id).Max(bk => (DateTime?)bk.TransactionDate),
                b.PaymentDate,
                b.CreatedDate
            });

            // Search filter
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

            // Custom Filters (e.g., from UI filters)
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
                        case "paymentstatus":
                            if (valStr.Equals("paid", StringComparison.OrdinalIgnoreCase))
                            {
                                query = query.Where(x => (x.TotalPaidCashAmount + x.TotalPaidBkashAmount + x.TotalPaidBankAmount) >= x.TotalAmount && x.TotalAmount > 0);
                            }
                            else if (valStr.Equals("partial", StringComparison.OrdinalIgnoreCase))
                            {
                                query = query.Where(x => (x.TotalPaidCashAmount + x.TotalPaidBkashAmount + x.TotalPaidBankAmount) > 0 && (x.TotalPaidCashAmount + x.TotalPaidBkashAmount + x.TotalPaidBankAmount) < x.TotalAmount);
                            }
                            else if (valStr.Equals("unpaid", StringComparison.OrdinalIgnoreCase) || valStr.Equals("due", StringComparison.OrdinalIgnoreCase))
                            {
                                query = query.Where(x => (x.TotalPaidCashAmount + x.TotalPaidBkashAmount + x.TotalPaidBankAmount) <= 0);
                            }
                            break;
                    }
                }
            }

            var totalRecord = await query.CountAsync(cancellationToken);

            // Sorting
            var sortCol = pagedRequest.SortColumn?.Trim().ToLower() ?? "createddate";
            bool isDesc = string.Equals(pagedRequest.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

            query = sortCol switch
            {
                "studentname" => isDesc ? query.OrderByDescending(x => x.StudentName) : query.OrderBy(x => x.StudentName),
                "stdcid" => isDesc ? query.OrderByDescending(x => x.StdCID) : query.OrderBy(x => x.StdCID),
                "totalamount" => isDesc ? query.OrderByDescending(x => x.TotalAmount) : query.OrderBy(x => x.TotalAmount),
                "totalpaidcashamount" => isDesc ? query.OrderByDescending(x => x.TotalPaidCashAmount) : query.OrderBy(x => x.TotalPaidCashAmount),
                "totalpaidbkashamount" => isDesc ? query.OrderByDescending(x => x.TotalPaidBkashAmount) : query.OrderBy(x => x.TotalPaidBkashAmount),
                "billyear" => isDesc ? query.OrderByDescending(x => x.BillYear).ThenByDescending(x => x.BillMonth) : query.OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth),
                "billmonth" => isDesc ? query.OrderByDescending(x => x.BillMonth) : query.OrderBy(x => x.BillMonth),
                _ => isDesc ? query.OrderByDescending(x => x.CreatedDate) : query.OrderByDescending(x => x.CreatedDate)
            };

            // Paging
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }
            else if (pagedRequest.PageSize > 0)
            {
                query = query.Take(pagedRequest.PageSize);
            }

            var rawItems = await query.ToListAsync(cancellationToken);

            var monthNames = new[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var items = rawItems.Select(x =>
            {
                // Calculate latest transaction date from CashBook or BankBook
                DateTime? trxDate = null;
                if (x.CashTrxDate.HasValue && x.BankTrxDate.HasValue)
                {
                    trxDate = x.CashTrxDate > x.BankTrxDate ? x.CashTrxDate : x.BankTrxDate;
                }
                else
                {
                    trxDate = x.CashTrxDate ?? x.BankTrxDate ?? x.PaymentDate;
                }

                decimal totalPaid = x.TotalPaidCashAmount + x.TotalPaidBkashAmount + x.TotalPaidBankAmount;
                decimal totalDue = Math.Max(0m, x.TotalAmount - totalPaid);

                string status;
                if (x.TotalAmount == 0)
                {
                    status = "Paid";
                }
                else if (totalPaid <= 0)
                {
                    status = "Unpaid";
                }
                else if (totalDue <= 0)
                {
                    status = "Paid";
                }
                else
                {
                    status = "Partial";
                }

                return new StudentBillHistoryResponse
                {
                    Id = x.Id,
                    AdmissionId = x.AdmissionId,
                    StudentId = x.StudentId,
                    StudentName = string.IsNullOrWhiteSpace(x.StudentName) ? "N/A" : x.StudentName,
                    StdCID = string.IsNullOrWhiteSpace(x.StdCID) ? "N/A" : x.StdCID,
                    BillMonth = x.BillMonth,
                    MonthName = x.BillMonth >= 1 && x.BillMonth <= 12 ? monthNames[x.BillMonth] : x.BillMonth.ToString(),
                    BillYear = x.BillYear,
                    VoucherNo = x.VoucherNo,
                    TotalAmount = x.TotalAmount,
                    TotalPaidAmount = totalPaid,
                    TotalDueAmount = totalDue,
                    TotalPaidBkashAmount = x.TotalPaidBkashAmount,
                    TotalPaidCashAmount = x.TotalPaidCashAmount,
                    TotalPaidBankAmount = x.TotalPaidBankAmount,
                    TransactionDate = trxDate,
                    FormattedTransactionDate = trxDate?.ToString("dd MMM yyyy hh:mm tt"),
                    PaymentStatus = status,
                    CreatedDate = x.CreatedDate
                };
            }).ToList();

            // Client-side sort if sorted by derived properties
            if (sortCol == "totalpaidamount")
            {
                items = isDesc
                    ? items.OrderByDescending(x => x.TotalPaidAmount).ToList()
                    : items.OrderBy(x => x.TotalPaidAmount).ToList();
            }
            else if (sortCol == "totaldueamount")
            {
                items = isDesc
                    ? items.OrderByDescending(x => x.TotalDueAmount).ToList()
                    : items.OrderBy(x => x.TotalDueAmount).ToList();
            }
            else if (sortCol == "transactiondate")
            {
                items = isDesc
                    ? items.OrderByDescending(x => x.TransactionDate).ToList()
                    : items.OrderBy(x => x.TransactionDate).ToList();
            }

            return Result.Success(new PagedResult<StudentBillHistoryResponse>
            {
                Items = items,
                TotalRecord = totalRecord,
                Page = pagedRequest.Page,
                PageSize = pagedRequest.PageSize
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<PagedResult<StudentBillHistoryResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
