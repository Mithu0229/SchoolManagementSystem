using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.PayBills.Models;
using System.Text.Json;

namespace SchoolManagementSystem.Application.School.PayBills.Handlers.QueryHandlers;

public class TSQQueryHandler : IHttpRequestHandler<TSQQueryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public TSQQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(TSQQueryCommand queryRequest, CancellationToken cancellationToken)
    {
        var req = queryRequest.Request;

        // 1. Mandatory Field Check (Code 406)
        if (string.IsNullOrWhiteSpace(req.UserName) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(req.TrxId))
        {
            return Result.Fail<TSQResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
        }

        // 2. Authentication Check (Code 403)
        if (await ValidateCredentials(req.UserName, req.Password) == false)
        {
            return Result.Fail<BkashTransactionResponse>(StatusCodes.Status403Forbidden, "Authentication failed");
        }

        try
        {
            var trxId = req.TrxId.Trim();

            // Search in BankBook first (or BkashTransaction)
            var bankBook = await _unitOfWork.BankBookRepository.GetAllNoneDeleted(false, true)
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

                var entity = new TSQResponse
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

                return Result.Success(entity, "Successful " + AlertMessage.SaveMessage);
            }

            // Search in BkashTransaction fallback
            var bkashTx = await _unitOfWork.BkashTransactionRepository.GetAllNoneDeleted(false, true)
                .Where(x => x.Remarks.Contains(trxId))
                .FirstOrDefaultAsync(cancellationToken);

            if (bkashTx != null)
            {
                var payTimeStr = bkashTx.Date.ToString("yyyyMMddHHmmss");

                var bkObj = new TSQResponse
                {
                    ErrorCode = "200",
                    ErrorMsg = "Successful",
                    TotalAmount = bkashTx.Amount.ToString("0.##"),
                    TrxId = trxId,
                    MiddlewarePayTime = payTimeStr,
                    RefNumber = bkashTx.Id.ToString(),
                    CustomMessage = "{Status: Success}"
                };

                return Result.Success(bkObj, "Successful " + AlertMessage.SaveMessage);

            }

            // If not found
            return Result.Fail(new TSQResponse
            {
                ErrorCode = "404",
                ErrorMsg = "Data not found"
            });
        }
        catch (Exception ex)
        {
            return Result.Fail(new TSQResponse
            {
                ErrorCode = "435",
                ErrorMsg = $"Data Mismatch: {ex.Message}"
            });
        }
    }

    private async Task<bool> ValidateCredentials(string userName, string password)
    {
        var user = await _unitOfWork.UserRepository.GetAllNoneDeleted(false, true).FirstOrDefaultAsync(x => x.Email == userName);
        if (user == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

}
