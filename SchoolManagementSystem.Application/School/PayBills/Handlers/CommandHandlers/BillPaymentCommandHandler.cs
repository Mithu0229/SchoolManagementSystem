using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.PayBills.Commands;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Domain.Enums;
using System.Text.Json;

namespace SchoolManagementSystem.Application.School.PayBills.Handlers.CommandHandlers;

public class BillPaymentCommandHandler : IRequestHandler<BillPaymentCommand, BillPaymentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public BillPaymentCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<BillPaymentResponse> Handle(BillPaymentCommand commandRequest, CancellationToken cancellationToken)
    {
        var req = commandRequest.Request;
        var refId = req.GetReferenceId();

        // 1. Mandatory Field Check (Code 406)
        if (string.IsNullOrWhiteSpace(req.UserName) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(refId) ||
            string.IsNullOrWhiteSpace(req.BillMonth) ||
            string.IsNullOrWhiteSpace(req.Amount) ||
            string.IsNullOrWhiteSpace(req.TrxId) ||
            string.IsNullOrWhiteSpace(req.PayTime))
        {
            return new BillPaymentResponse
            {
                ErrorCode = "406",
                ErrorMsg = "Mandatory Field missing"
            };
        }

        // 2. Authentication Check (Code 403)
        if (!ValidateCredentials(req.UserName, req.Password))
        {
            return new BillPaymentResponse
            {
                ErrorCode = "403",
                ErrorMsg = "Authentication failed"
            };
        }

        // 3. Parse BillMonth (MMYYYY)
        if (!TryParseBillMonth(req.BillMonth, out int month, out int year))
        {
            return new BillPaymentResponse
            {
                ErrorCode = "435",
                ErrorMsg = "Data Mismatch"
            };
        }

        // 4. Parse Amount
        if (!decimal.TryParse(req.Amount, out var payAmount) || payAmount <= 0)
        {
            return new BillPaymentResponse
            {
                ErrorCode = "406",
                ErrorMsg = "Mandatory Field missing"
            };
        }

        try
        {
            // 5. Duplicate Transaction Check (Code 436)
            var existingTx = await _unitOfWork.BkashTransactionRepository.GetAllNoneDeleted()
                .FirstOrDefaultAsync(x => x.Remarks.Contains(req.TrxId), cancellationToken);
            if (existingTx != null)
            {
                return new BillPaymentResponse
                {
                    ErrorCode = "436",
                    ErrorMsg = "Already paid"
                };
            }

            var existingBankBook = await _unitOfWork.BankBookRepository.GetAllNoneDeleted()
                .FirstOrDefaultAsync(x => x.TransactionNo == req.TrxId || x.VoucherNo == req.TrxId, cancellationToken);
            if (existingBankBook != null)
            {
                return new BillPaymentResponse
                {
                    ErrorCode = "436",
                    ErrorMsg = "Already paid"
                };
            }

            // 6. Lookup Bill
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
                return new BillPaymentResponse
                {
                    ErrorCode = "404",
                    ErrorMsg = "Data not found"
                };
            }

            // 7. Already Paid Check (Code 436)
            if (bill.IsActive)
            {
                return new BillPaymentResponse
                {
                    ErrorCode = "436",
                    ErrorMsg = "Already paid"
                };
            }

            // 8. Amount Check
            if (payAmount < bill.TotalAmount)
            {
                return new BillPaymentResponse
                {
                    ErrorCode = "438",
                    ErrorMsg = "Minimum amount not paid"
                };
            }

            if (payAmount != bill.TotalAmount)
            {
                return new BillPaymentResponse
                {
                    ErrorCode = "439",
                    ErrorMsg = "Pay amount and biller amount not match"
                };
            }

            // 9. Process Payment
            bill.IsActive = true;
            bill.TransactionType = TransactionType.Bkash;
            await _unitOfWork.BillMasterRepository.UpdateAsync(bill);

            // Record BkashTransaction
            var bkashTx = new BkashTransaction
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                TransactionType = "PayBill",
                Amount = payAmount,
                FromNumber = req.UserMobileNumber ?? "",
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

            return new BillPaymentResponse
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
        }
        catch (Exception ex)
        {
            return new BillPaymentResponse
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
