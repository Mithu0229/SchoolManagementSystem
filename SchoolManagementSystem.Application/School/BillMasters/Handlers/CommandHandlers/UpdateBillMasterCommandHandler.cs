using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class UpdateBillMasterCommandHandler : IHttpRequestHandler<UpdateBillMasterCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public UpdateBillMasterCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration) 
    { 
        _unitOfWork = unitOfWork; 
        _configuration = configuration;
    }
    public async Task<IResult> Handle(UpdateBillMasterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.BillMaster.Id == Guid.Empty) return Result.Fail<BillMasterResponse>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.BillMasterRepository.GetSingleNoneDeletedAsync(x => x.Id == request.BillMaster.Id);
            if (entity is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.BillMaster.Id && x.AdmissionId == request.BillMaster.AdmissionId && x.BillMonth == request.BillMaster.BillMonth && x.BillYear == request.BillMaster.BillYear)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Bill already exists for this admission, month and year!");
            entity.AdmissionId = request.BillMaster.AdmissionId;
            entity.BillMonth = request.BillMaster.BillMonth;
            entity.BillYear = request.BillMaster.BillYear;
            entity.TotalAmount = request.BillMaster.TotalAmount;
            entity.IsActive = true;
            entity.TransactionType = request.BillMaster.TransactionType;
            await _unitOfWork.BillMasterRepository.UpdateAsync(entity);
            var details = request.BillMaster.Details.Select(x => new BillDetail
            {
                BillMasterId = entity.Id,
                FeeTemplateDetailId = x.FeeTemplateDetailId,
                FeeHeadId = x.FeeHeadId,
                Amount = x.Amount
            }).ToList();
            await _unitOfWork.BillMasterRepository.ReplaceManyAsync<BillDetail>(x => x.BillMasterId == entity.Id, details);

            //cash book and bank book
            if (request.BillMaster.TransactionType == TransactionType.Cash)
            {
                // Debit Entry (Cash in)
                var cashBookDebit = new CashBook
                {
                    BillMasterId = entity.Id,
                    TransactionDate = DateTime.Now,
                    Debit = request.BillMaster.TotalAmount,
                    Credit = 0,
                    Balance = 0, // Should typically compute from previous balance
                    AccountNo = "Cash", // Default cash account representation
                    VoucherNo = request.BillMaster.VoucherNo ?? "",
                    Particulars = request.BillMaster.Particulars ?? ""
                };
                await _unitOfWork.CashBookRepository.AddAsync(cashBookDebit);

                // Credit Entry (Income / Accounts Receivable out)
                var cashBookCredit = new CashBook
                {
                    BillMasterId = entity.Id,
                    TransactionDate = DateTime.Now,
                    Debit = 0,
                    Credit = request.BillMaster.TotalAmount,
                    Balance = 0,
                    AccountNo = request.BillMaster.StdCID ?? "Student", // Student's account or Income account
                    VoucherNo = request.BillMaster.VoucherNo ?? "",
                    Particulars = "Bill Collection - " + (request.BillMaster.Particulars ?? "")
                };
                await _unitOfWork.CashBookRepository.AddAsync(cashBookCredit);
            }
            else if (request.BillMaster.TransactionType == TransactionType.Bank || request.BillMaster.TransactionType == TransactionType.Bkash)
            {
                // Debit Entry (Bank in)
                var bankBookDebit = new BankBook
                {
                    BillMasterId = entity.Id,
                    TransactionDate = DateTime.Now,
                    Debit = request.BillMaster.TotalAmount,
                    Credit = 0,
                    Balance = 0, // Should typically compute from previous balance
                    BankName = request.BillMaster.BankName ?? "",
                    AccountNo = request.BillMaster.AccountNo ?? "",
                    TransactionNo = request.BillMaster.TransactionNo ?? "",
                    TransactionType = request.BillMaster.TransactionType,
                    VoucherNo = request.BillMaster.VoucherNo ?? "",
                    Particulars = request.BillMaster.Particulars ?? ""
                };
                await _unitOfWork.BankBookRepository.AddAsync(bankBookDebit);

                // Credit Entry (Income / Accounts Receivable out)
                var bankBookCredit = new BankBook
                {
                    BillMasterId = entity.Id,
                    TransactionDate = DateTime.Now,
                    Debit = 0,
                    Credit = request.BillMaster.TotalAmount,
                    Balance = 0,
                    BankName = request.BillMaster.BankName ?? "",
                    AccountNo = request.BillMaster.StdCID ?? "Student", // Student's account or Income account
                    TransactionNo = request.BillMaster.TransactionNo ?? "",
                    TransactionType = request.BillMaster.TransactionType,
                    VoucherNo = request.BillMaster.VoucherNo ?? "",
                    Particulars = "Bill Collection - " + (request.BillMaster.Particulars ?? "")
                };
                await _unitOfWork.BankBookRepository.AddAsync(bankBookCredit);
            }

            
            //send sms
            try
            {
                string studentName = request.BillMaster.StdCID!;
                decimal paidAmount = request.BillMaster.TotalAmount;

                string message = $"The bill for student {studentName} has been paid successfully. Paid Amount: ৳{paidAmount:N2}.";

                var smsPayload = new
                {
                    apikey = _configuration["SmsSettings:ApiKey"],
                    secretkey = _configuration["SmsSettings:SecretKey"],
                    callerID = _configuration["SmsSettings:CallerID"],
                    toUser = "8801755948794",
                    messageContent = message
                };

                using var httpClient = new System.Net.Http.HttpClient();
                var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(smsPayload), System.Text.Encoding.UTF8, "application/json");
                // Note: Ensure the endpoint path (e.g. /api/v1/send or /smsapi) matches what Songbird Telecom expects
                var response = await httpClient.PostAsync("http://sms.songbirdtelecom.com:8746/sendtext", content);


                var student = await _unitOfWork.StudentInfoRepository.GetSingleNoneDeletedAsync(x=>x.StdCID == request.BillMaster.StdCID);
                // sms history
                var smsHistory = new SMSHistory
                {
                    Id = Guid.NewGuid(),
                    SMSType = "Payment",
                    Message = message,
                    Phone = student.StudentPhone!,
                    StudentId = student.Id,
                    IsActive = true

                };
                await _unitOfWork.SMSHistoryRepository.AddAsync(smsHistory);
                await _unitOfWork.CommitAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS Error: {ex.Message}");
            }

            return Result.Success(entity.Adapt<BillMasterResponse>(), "BillMaster " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<BillMasterResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
