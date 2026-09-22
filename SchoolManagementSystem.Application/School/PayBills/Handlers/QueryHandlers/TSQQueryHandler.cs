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
        if (string.IsNullOrWhiteSpace(req.TrxId))
        {
            return Result.Fail<TSQResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
        }

        try
        {
            var trxId = req.TrxId.Trim();

            // Search in BankBook first (or BkashTransaction)
            var bankBook = await _unitOfWork.BankBookRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.BillMaster)
                    .ThenInclude(b => b.Details)
                        .ThenInclude(d => d.FeeHead)
                .Where(x => x.TransactionNo == trxId || x.VoucherNo == trxId && x.Debit == 0)
                .ToListAsync(cancellationToken);
            if (bankBook.Count == 0)
            {
                return Result.Fail(404, "Data not found");
            }

            var entity = new TSQResponse
            {
                //ErrorCode = "200",
                //ErrorMsg = "Successful",
                TotalAmount = bankBook.Sum(x=>x.Credit).ToString("0.##"),
                TrxId = trxId,
                RefNumber = bankBook.FirstOrDefault()!.AccountNo.ToString(),
                //CustomMessage = "{Status: Success}",
                MiddlewarePayTime =bankBook.FirstOrDefault()!.TransactionDate.TimeOfDay.ToString()
            };

            return Result.Success(entity, "Successful ",200);

        }
        catch (Exception ex)
        {
            return Result.Fail(500, ex.Message);
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
