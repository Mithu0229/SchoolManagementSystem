using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using SchoolManagementSystem.Domain.Enums;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetMultiMonthMoneyReceiptQueryHandler : IHttpRequestHandler<GetMultiMonthMoneyReceiptQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMultiMonthMoneyReceiptQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetMultiMonthMoneyReceiptQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var bills = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false,true)
                .Include(x => x.Admission)
                .ThenInclude(x => x.Student)
                .Include(x => x.Admission)
                .ThenInclude(x => x.Class)
                .Where(x => x.VoucherNo == request.VoucherNo)
                .OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth)
                .ToListAsync(cancellationToken);

            if (!bills.Any())
                return Result.Fail<MultiMonthMoneyReceiptResponse>(StatusCodes.Status404NotFound, "Receipt not found");

            var firstBill = bills.First();
            var student = firstBill.Admission.Student;
            var admission = firstBill.Admission;

            var res = new MultiMonthMoneyReceiptResponse
            {
                ReceiptNo = firstBill.VoucherNo ?? "",
                Date = DateTime.Now.ToString("dd-MMM-yyyy"),
                StudentName = $"{student.StudentEmail} ({student.StdCID})",
                AdmissionNo = admission.RollNo,
                ClassName = admission.Class?.ClassName ?? "",
                PaymentMethod = firstBill.TransactionType.ToString(),
                TransactionType = "Debit", // From the prompt
                TotalAmount = bills.Sum(x => x.TotalAmount),
                CollectionAmount = bills.Sum(x => x.CollectionAmount),
                DueAmount = bills.Sum(x => x.DueAmount),
                Details = bills.Select(x => new MultiMonthReceiptDetail
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.BillMonth),
                    Total = x.TotalAmount,
                    Collection = x.CollectionAmount,
                    Due = x.DueAmount
                }).ToList()
            };

            return Result.Success(res);
        }
        catch (Exception ex)
        {
            return Result.Fail<MultiMonthMoneyReceiptResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
