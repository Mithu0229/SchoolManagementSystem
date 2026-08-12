using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Application.School.PayBills.Queries;
using System.Text.Json;

namespace SchoolManagementSystem.Application.School.PayBills.Handlers.QueryHandlers;

public class TSQQueryHandler : IRequestHandler<TSQQuery, TSQResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public TSQQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<TSQResponse> Handle(TSQQuery queryRequest, CancellationToken cancellationToken)
    {
        var req = queryRequest.Request;

        // 1. Mandatory Field Check (Code 406)
        if (string.IsNullOrWhiteSpace(req.UserName) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(req.TrxId))
        {
            return new TSQResponse
            {
                ErrorCode = "406",
                ErrorMsg = "Mandatory Field missing"
            };
        }

        // 2. Authentication Check (Code 403)
        if (!ValidateCredentials(req.UserName, req.Password))
        {
            return new TSQResponse
            {
                ErrorCode = "403",
                ErrorMsg = "Authentication failed"
            };
        }

        try
        {
            var trxId = req.TrxId.Trim();

            // Search in BankBook first (or BkashTransaction)
            var bankBook = await _unitOfWork.BankBookRepository.GetAllNoneDeleted()
                .Include(x => x.BillMaster)
                    .ThenInclude(b => b.Details)
                        .ThenInclude(d => d.FeeHead)
                .Where(x => x.TransactionNo == trxId || x.VoucherNo == trxId)
                .FirstOrDefaultAsync(cancellationToken);

            if (bankBook != null)
            {
                string? breakdownStr = null;
                if (bankBook.BillMaster?.Details != null && bankBook.BillMaster.Details.Any())
                {
                    var breakdownDict = bankBook.BillMaster.Details
                        .Where(d => d.FeeHead != null)
                        .ToDictionary(
                            d => d.FeeHead?.FeeHeadName ?? "Fee",
                            d => (int)d.Amount
                        );
                    breakdownStr = JsonSerializer.Serialize(breakdownDict);
                }

                var payTimeStr = bankBook.TransactionDate.ToString("yyyyMMddHHmmss");
                var amountVal = bankBook.Debit > 0 ? bankBook.Debit : bankBook.Credit;

                return new TSQResponse
                {
                    ErrorCode = "200",
                    ErrorMsg = "Successful",
                    TotalAmount = amountVal.ToString("0.##"),
                    TrxId = trxId,
                    MiddlewarePayTime = payTimeStr,
                    RefNumber = bankBook.BillMasterId.ToString(),
                    CustomMessage = "{Status: Success}",
                    AmountBreakdown = breakdownStr
                };
            }

            // Search in BkashTransaction fallback
            var bkashTx = await _unitOfWork.BkashTransactionRepository.GetAllNoneDeleted()
                .Where(x => x.Remarks.Contains(trxId))
                .FirstOrDefaultAsync(cancellationToken);

            if (bkashTx != null)
            {
                var payTimeStr = bkashTx.Date.ToString("yyyyMMddHHmmss");

                return new TSQResponse
                {
                    ErrorCode = "200",
                    ErrorMsg = "Successful",
                    TotalAmount = bkashTx.Amount.ToString("0.##"),
                    TrxId = trxId,
                    MiddlewarePayTime = payTimeStr,
                    RefNumber = bkashTx.Id.ToString(),
                    CustomMessage = "{Status: Success}"
                };
            }

            // If not found
            return new TSQResponse
            {
                ErrorCode = "404",
                ErrorMsg = "Data not found"
            };
        }
        catch (Exception ex)
        {
            return new TSQResponse
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

        return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password);
    }
}
