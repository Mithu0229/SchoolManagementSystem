using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Application.School.PayBills.Queries;
using System.Text.Json;

namespace SchoolManagementSystem.Application.School.PayBills.Handlers.QueryHandlers;

public class CheckBillQueryHandler : IRequestHandler<CheckBillQuery, CheckBillResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public CheckBillQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<CheckBillResponse> Handle(CheckBillQuery queryRequest, CancellationToken cancellationToken)
    {
        var req = queryRequest.Request;
        var refId = req.GetReferenceId();

        // 1. Mandatory Field Check (Code 406)
        if (string.IsNullOrWhiteSpace(req.UserName) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(refId) ||
            string.IsNullOrWhiteSpace(req.BillMonth))
        {
            return new CheckBillResponse
            {
                ErrorCode = "406",
                ErrorMsg = "Mandatory Field missing"
            };
        }

        // 2. Authentication Check (Code 403)
        if (!ValidateCredentials(req.UserName, req.Password))
        {
            return new CheckBillResponse
            {
                ErrorCode = "403",
                ErrorMsg = "Authentication failed"
            };
        }

        // 3. Parse BillMonth (MMYYYY)
        if (!TryParseBillMonth(req.BillMonth, out int month, out int year))
        {
            return new CheckBillResponse
            {
                ErrorCode = "435",
                ErrorMsg = "Data Mismatch"
            };
        }

        try
        {
            // 4. Lookup Bill
            var searchRef = refId.Trim().ToLower();
            var bill = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Include(x => x.Admission)
                    .ThenInclude(a => a.Student)
                .Include(x => x.Details)
                    .ThenInclude(d => d.FeeHead)
                .Where(x => x.BillMonth == month && x.BillYear == year &&
                            ((x.Admission != null && x.Admission.Student != null && x.Admission.Student.StdCID.ToLower() == searchRef) ||
                             (x.Admission != null && x.Admission.RollNo.ToLower() == searchRef) ||
                             x.Id.ToString().ToLower() == searchRef))
                .FirstOrDefaultAsync(cancellationToken);

            if (bill == null)
            {
                return new CheckBillResponse
                {
                    ErrorCode = "404",
                    ErrorMsg = "Data not found"
                };
            }

            // 5. Already Paid Check (Code 436)
            if (bill.IsActive)
            {
                return new CheckBillResponse
                {
                    ErrorCode = "436",
                    ErrorMsg = "Already paid"
                };
            }

            // 6. Amount Check (if provided)
            if (!string.IsNullOrWhiteSpace(req.Amount))
            {
                if (decimal.TryParse(req.Amount, out var requestedAmount))
                {
                    if (requestedAmount < bill.TotalAmount)
                    {
                        return new CheckBillResponse
                        {
                            ErrorCode = "438",
                            ErrorMsg = "Minimum amount not paid"
                        };
                    }

                    if (requestedAmount != bill.TotalAmount)
                    {
                        return new CheckBillResponse
                        {
                            ErrorCode = "439",
                            ErrorMsg = "Pay amount and biller amount not match"
                        };
                    }
                }
            }

            // Build Amount Breakdown
            string? breakdownStr = null;
            if (bill.Details != null && bill.Details.Any())
            {
                var breakdownDict = bill.Details
                    .Where(d => d.FeeHead != null)
                    .ToDictionary(
                        d => d.FeeHead?.FeeHeadName ?? "Fee",
                        d => (int)d.Amount
                    );
                breakdownStr = JsonSerializer.Serialize(breakdownDict);
            }

            // Bill Due Date (last day of the bill month)
            var lastDayOfMonth = DateTime.DaysInMonth(year, month);
            var dueDateStr = new DateTime(year, month, lastDayOfMonth).ToString("yyyyMMdd");
            var queryTimeStr = DateTime.Now.ToString("yyyyMMddHHmmss");

            var consumerName = bill.Admission?.Student?.FullName;
            if (string.IsNullOrWhiteSpace(consumerName))
            {
                consumerName = bill.Admission?.Student?.StdCID;
            }

            return new CheckBillResponse
            {
                ErrorCode = "200",
                ErrorMsg = "Successful",
                ConsumerName = consumerName,
                BillMonth = req.BillMonth,
                BillAmount = bill.TotalAmount.ToString("0.##"),
                BillDueDate = dueDateStr,
                QueryTime = queryTimeStr,
                AmountBreakdown = breakdownStr
            };
        }
        catch (Exception ex)
        {
            return new CheckBillResponse
            {
                ErrorCode = "435",
                ErrorMsg = $"Data Mismatch: {ex.Message}"
            };
        }
    }

    private bool ValidateCredentials(string userName, string password)
    {
        var configuredUser = _configuration["PayBillSettings:UserName"];
        var configuredPass = _configuration["PayBillSettings:Password"];

        if (!string.IsNullOrEmpty(configuredUser) && !string.IsNullOrEmpty(configuredPass))
        {
            return string.Equals(userName, configuredUser, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(password, configuredPass);
        }

        // Fallback: check not null/empty
        return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password);
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
