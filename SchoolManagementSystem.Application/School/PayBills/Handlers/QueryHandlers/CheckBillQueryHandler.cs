using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.Application.School.PayBills.Handlers.QueryHandlers;

public class CheckBillQueryHandler : IHttpRequestHandler<CheckBillCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public CheckBillQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(CheckBillCommand queryRequest, CancellationToken cancellationToken)
    {
        var req = queryRequest.Request;
        var refId = req.GetReferenceId();

        // 1. Mandatory Field Check (Code 406)
        if (string.IsNullOrWhiteSpace(refId) ||
            string.IsNullOrWhiteSpace(req.BillMonth))
        {
            return Result.Fail<CheckBillResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
        }

        // 2. Authentication Check (Code 403)

        var student = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(false, true).FirstOrDefaultAsync(x => x.StdCID == req.RefID);
        if (student == null)
        {
            return Result.Fail<CheckBillResponse>(StatusCodes.Status403Forbidden, "Authentication Failed");
        }

        // 3. Parse BillMonth (MMYYYY)
        if (!TryParseBillMonth(req.BillMonth, out int month, out int year))
        {
            return Result.Fail<CheckBillResponse>(435, "Data Mismatch");
        }

        var billDate = new DateTime(year, month, 1);
        var currentMonth = new DateTime(year, month, 1);

        if (billDate > currentMonth)
        {
            return Result.Fail<BkashTransactionResponse>(437, "Due date over");
        }

        try
        {
            var searchRef = refId.Trim().ToLower();
            var bill = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Admission)
                    .ThenInclude(a => a.Student)
                .Include(x => x.Details)
                    .ThenInclude(d => d.FeeHead)
                .Where(x => (x.BillYear < year ||
                            (x.BillYear == year && x.BillMonth <= month))
                            && !x.IsPaid && !x.IsActive &&
                            ((x.Admission != null && x.Admission.Student != null && x.Admission.Student.StdCID!.ToLower() == req.RefID)))
                .ToListAsync(cancellationToken);

            if (bill.Count == 0)
            {
                var Paidbill = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Admission)
                    .ThenInclude(a => a.Student)
                .Include(x => x.Details)
                    .ThenInclude(d => d.FeeHead)
                .Where(x => (x.BillYear < year ||
                            (x.BillYear == year && x.BillMonth <= month))
                            && x.IsPaid && x.IsActive &&
                            ((x.Admission != null && x.Admission.Student != null && x.Admission.Student.StdCID!.ToLower() == req.RefID)))
                .ToListAsync(cancellationToken);
                if (Paidbill.Count > 0)
                {
                    return Result.Fail<CheckBillResponse>(436, "Already paid");
                }
                return Result.Fail<CheckBillResponse>(404, "Data not found");
            }

            // Bill Due Date (last day of the bill month)
            var lastDayOfMonth = DateTime.DaysInMonth(year, month);
            var dueDateStr = new DateTime(year, month, lastDayOfMonth).ToString("yyyyMMdd");
            var queryTimeStr = DateTime.Now.ToString("yyyyMMddHHmmss");

            var consumerName = bill.FirstOrDefault()!.Admission?.Student?.FullName;
            if (string.IsNullOrWhiteSpace(consumerName))
            {
                consumerName = bill.FirstOrDefault()!.Admission?.Student?.StdCID;
            }
            return Result.Success(new CheckBillResponse
            {
                ConsumerName = consumerName,
                BillMonth = req.BillMonth,
                BillAmount = (bill.Sum(x => x.TotalAmount) - (bill.Sum(x => x.CollectionAmount))).ToString("0.##"),

            }, "Success", 200);

        }
        catch (Exception ex)
        {
            return Result.Fail<CheckBillResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    
    private bool TryParseBillMonth(string billMonth, out int month, out int year)
    {
        month = 0;
        year = 0;
        if (string.IsNullOrWhiteSpace(billMonth) || billMonth.Length != 6) return false;

        if (int.TryParse(billMonth.Substring(0, 2), out month) &&
            int.TryParse(billMonth.Substring(2, 4), out year))
        {
            return month >= 1 && month <= 12 && year >= 2000 && year <= 2100;
        }

        return false;
    }
}
