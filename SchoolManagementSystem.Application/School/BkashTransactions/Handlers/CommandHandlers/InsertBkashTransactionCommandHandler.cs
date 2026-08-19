using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Domain.Enums;
using System.Text.Json;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.CommandHandlers;

public class InsertBkashTransactionCommandHandler : IHttpRequestHandler<InsertBkashTransactionCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public InsertBkashTransactionCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<IResult> Handle(InsertBkashTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable);

            var req = request.BkashTransaction;

            // 1. Mandatory Field Check (Code 406)
            if (string.IsNullOrWhiteSpace(req.UserName) ||
                string.IsNullOrWhiteSpace(req.Password) ||
                string.IsNullOrWhiteSpace(req.BillMonth) ||
                string.IsNullOrWhiteSpace(req.Amount.ToString()) ||
                string.IsNullOrWhiteSpace(req.TrxId) ||
                string.IsNullOrWhiteSpace(req.PayTime))
            {
                return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
            }

            // 2. Authentication Check (Code 403)
            if (await ValidateCredentials(req.UserName, req.Password) == false)
            {
                return Result.Fail<BkashTransactionResponse>(StatusCodes.Status403Forbidden, "Authentication failed");
            }

            // 3. Parse BillMonth (MMYYYY)
            if (!TryParseBillMonth(req.BillMonth, out int month, out int year))
            {
                return Result.Fail<BkashTransactionResponse>(435, "Data Mismatch");

            }

            // 4. Parse Amount
            if (!decimal.TryParse(req.Amount.ToString(), out var payAmount) || payAmount <= 0)
            {
                return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
            }

            try
            {
                // 5. Duplicate Transaction Check (Code 436)
                var existingTx = await _unitOfWork.BkashTransactionRepository.GetAllNoneDeleted(false,true)
                    .FirstOrDefaultAsync(x => x.Remarks.Contains(req.TrxId), cancellationToken);
                if (existingTx != null)
                {
                    return Result.Fail<BkashTransactionResponse>(436, "Already paid");
                }

                // 6. Lookup Bill
                var user = await _unitOfWork.UserRepository.GetAllNoneDeleted(false, true).FirstOrDefaultAsync(x => x.Email == req.UserName);
                if (user != null)
                {

                    var bill = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                    .Include(x => x.Admission)
                        .ThenInclude(a => a.Student)
                    .Include(x => x.Details)
                        .ThenInclude(d => d.FeeHead)
                    .Where(x => x.BillMonth == month && x.BillYear == year && x.Admission.StudentId == user.StudentId)
                    .FirstOrDefaultAsync(cancellationToken);

                    if (bill == null)
                    {
                        return Result.Fail<BkashTransactionResponse>(404, "Data not found");
                    }

                    // 7. Already Paid Check (Code 436)
                    if (bill.IsActive)
                    {
                        return Result.Fail<BkashTransactionResponse>(436, "Already paid");
                    }

                    // 8. Amount Check
                    if (payAmount < bill.TotalAmount)
                    {
                        return Result.Fail<BkashTransactionResponse>(438, "Minimum amount not paid");
                    }

                    if (payAmount != bill.TotalAmount)
                    {
                        return Result.Fail<BkashTransactionResponse>(439, "Pay amount and biller amount not match");
                    }

                    // 9. Process Payment
                    bill.IsActive = true;
                    bill.TransactionType = TransactionType.Bkash;
                    await _unitOfWork.BillMasterRepository.UpdateAsync(bill);

                    // Record BkashTransaction
                    var bkashTx = new BkashTransaction
                    {
                        Id = Guid.NewGuid(),
                        CustomerNo = req.CustomerNo,
                        BillMonth = req.BillMonth,
                        UserMobileNumber = req.UserMobileNumber,
                        TrxId = req.TrxId,
                        PayTime = req.PayTime,
                        Date = DateTime.Now,
                        TransactionType = "PayBill",
                        Amount = payAmount,
                        Remarks = $"TrxId:{req.TrxId}, StdCID:{bill.Admission?.Student?.StdCID}, Month:{req.BillMonth}",
                        IsActive = true
                    };
                    await _unitOfWork.BkashTransactionRepository.AddAsync(bkashTx);

                    // Record BankBook Debit and Credit Entries
                    var bankBookDebit = new BankBook
                    {
                        BillMasterId = bill.Id,
                        TransactionDate = DateTime.Now,
                        Debit = payAmount,
                        Credit = 0,
                        Balance = 0,
                        BankName = "bKash",
                        AccountNo = req.UserMobileNumber ?? "bKash",
                        TransactionNo = req.TrxId,
                        TransactionType = TransactionType.Bkash,
                        VoucherNo = req.TrxId,
                        Particulars = $"bKash Pay Bill - Std: {bill.Admission?.Student?.StdCID}"
                    };
                    await _unitOfWork.BankBookRepository.AddAsync(bankBookDebit);

                    var bankBookCredit = new BankBook
                    {
                        BillMasterId = bill.Id,
                        TransactionDate = DateTime.Now,
                        Debit = 0,
                        Credit = payAmount,
                        Balance = 0,
                        BankName = "bKash",
                        AccountNo = bill.Admission?.Student?.StdCID ?? "Student",
                        TransactionNo = req.TrxId,
                        TransactionType = TransactionType.Bkash,
                        VoucherNo = req.TrxId,
                        Particulars = $"Bill Collection - bKash Trx: {req.TrxId}"
                    };
                    await _unitOfWork.BankBookRepository.AddAsync(bankBookCredit);

                    // Send SMS notification if configured
                    var studentPhone = bill.Admission?.Student?.StudentPhone;
                    if (!string.IsNullOrWhiteSpace(studentPhone))
                    {
                        try
                        {
                            string stdName = bill.Admission?.Student?.FullName ?? bill.Admission?.Student?.StdCID ?? "Student";
                            string smsMessage = $"The bill for student {stdName} has been paid successfully via bKash. TrxId: {req.TrxId}, Paid Amount: ৳{payAmount:N2}.";

                            var smsPayload = new
                            {
                                apikey = _configuration["SmsSettings:ApiKey"],
                                secretkey = _configuration["SmsSettings:SecretKey"],
                                callerID = _configuration["SmsSettings:CallerID"],
                                toUser = studentPhone,
                                messageContent = smsMessage
                            };

                            using var httpClient = new System.Net.Http.HttpClient();
                            var content = new System.Net.Http.StringContent(JsonSerializer.Serialize(smsPayload), System.Text.Encoding.UTF8, "application/json");
                            await httpClient.PostAsync("http://sms.songbirdtelecom.com:8746/sendtext", content);

                            if (bill.Admission?.Student != null)
                            {
                                var smsHistory = new SMSHistory
                                {
                                    Id = Guid.NewGuid(),
                                    SMSType = "bKash PayBill",
                                    Message = smsMessage,
                                    Phone = studentPhone,
                                    StudentId = bill.Admission.Student.Id,
                                    IsActive = true
                                };
                                await _unitOfWork.SMSHistoryRepository.AddAsync(smsHistory);
                            }

                        }
                        catch (Exception smsEx)
                        {
                            Console.WriteLine($"SMS Error during PayBill: {smsEx.Message}");
                        }
                    }

                    await _unitOfWork.CommitAsync(cancellationToken);

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

                    var consumerName = bill.Admission?.Student?.FullName;
                    if (string.IsNullOrWhiteSpace(consumerName))
                    {
                        consumerName = bill.Admission?.Student?.StdCID;
                    }

                    var middlewarePayTimeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var entity = new BillPaymentResponse
                    {
                        ErrorCode = "200",
                        ErrorMsg = "Successful",
                        ConsumerName = consumerName,
                        TotalAmount = payAmount.ToString("0.##"),
                        TrxId = req.TrxId,
                        MiddlewarePayTime = middlewarePayTimeStr,
                        RefNumber = bill.Id.ToString(),
                        CustomMessage = "{Token: Paid successfully}",
                        AmountBreakdown = breakdownStr
                    };

                    return Result.Success(entity, "BkashTransaction " + AlertMessage.SaveMessage);

                }
            }
            catch (Exception ex)
            {
                return Result.Fail<BkashTransactionResponse>(435, $"Data Mismatch: {ex.Message}");
            }

            return Result.Fail<BkashTransactionResponse>(435, "Data Mismatch");
        }
        catch (Exception ex)
        {
            return Result.Fail<BkashTransactionResponse>(435, $"Data Mismatch: {ex.Message}");
        }
    }

    private async Task<bool> ValidateCredentials(string userName, string password)
    {
        var user = await _unitOfWork.UserRepository.GetAllNoneDeleted(false,true).FirstOrDefaultAsync(x => x.Email == userName);
        if (user == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.Password);
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



