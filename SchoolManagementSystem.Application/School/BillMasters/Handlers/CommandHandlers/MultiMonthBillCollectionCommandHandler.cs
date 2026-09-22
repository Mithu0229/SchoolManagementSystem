using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class MultiMonthBillCollectionCommandHandler : IHttpRequestHandler<MultiMonthBillCollectionCommand>//IHttpRequestHandler<MultiMonthBillCollectionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public MultiMonthBillCollectionCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<IResult> Handle(MultiMonthBillCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.Request;

            if (req == null || !req.BillMasterIds.Any() || req.CollectionAmount <= 0)
                return Result.Fail<MultiMonthBillCollectionResponse>(StatusCodes.Status406NotAcceptable, "Invalid request");

            var bills = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
                .Include(x => x.Admission)
                .ThenInclude(x => x.Student)
                .Include(x => x.Details)
                .Where(x => req.BillMasterIds.Contains(x.Id) && !x.IsPaid) // Check IsPaid instead of IsActive
                .OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth)
                .ToListAsync(cancellationToken);
            if (bills.Count > 0)
            {
                if ((bills.Sum(x => x.TotalAmount)- bills.Sum(x => x.CollectionAmount)) < req.CollectionAmount)
                {
                    var t = bills.Sum(x => x.TotalAmount);
                    var s = bills.Sum(x => x.CollectionAmount);
                    return Result.Fail<MultiMonthBillCollectionResponse>(439, "Pay amount and biller amount not match");
                }
                string stdCID = bills.First().Admission.Student.StdCID;

                if (!bills.Any())
                    return Result.Fail<MultiMonthBillCollectionResponse>(StatusCodes.Status404NotFound, "No unpaid bills found");
                // 2. Process Additional Details (ad-hoc charges, fines, etc.) onto the first bill
                if (req.AdditionalDetails != null && req.AdditionalDetails.Any())
                {
                    var firstBill = bills.First();
                    firstBill.Details ??= new List<BillDetail>();
                    decimal extraAmount = 0;
                    foreach (var detail in req.AdditionalDetails)
                    {
                        firstBill.Details.Add(new BillDetail
                        {
                            BillMasterId = firstBill.Id,
                            FeeHeadId = detail.FeeHeadId,
                            Amount = detail.Amount,
                            FeeTemplateDetailId = detail.FeeTemplateDetailId != Guid.Empty ? detail.FeeTemplateDetailId : Guid.Empty
                        });
                        extraAmount += detail.Amount;
                    }
                    // Add to TotalAmount; DueAmount will be calculated during the distribution below
                    firstBill.TotalAmount += extraAmount;
                }
                decimal remainingCollection = req.CollectionAmount;
                int paidBillsCount = 0;
                DateTime now = DateTime.Now;
                string voucherNo = req.VoucherNo ?? $"V-{now:yyyyMMddHHmmss}";
                // 3. Distribute collection across bills (FIFO)
                foreach (var bill in bills)
                {
                    // Stop once the collected amount has been fully allocated
                    if (remainingCollection <= 0)
                        break;
                    // Outstanding amount for this specific month
                    decimal currentDue = bill.TotalAmount - bill.CollectionAmount;
                    if (currentDue <= 0)
                    {
                        // Already fully covered
                        bill.IsPaid = true;
                        bill.DueAmount = 0;
                        continue;
                    }
                    // Amount to allocate to this month
                    decimal amountToPay = Math.Min(remainingCollection, currentDue);
                    // Update bill fields
                    bill.CollectionAmount += amountToPay;
                    bill.DueAmount = bill.TotalAmount - bill.CollectionAmount; // Remaining due after payment
                    bill.PaymentDate = now;
                    bill.VoucherNo = voucherNo;
                    bill.TransactionType = req.TransactionType;
                    // Check payment completion status
                    if (bill.DueAmount <= 0)
                    {
                        bill.IsPaid = true;
                        bill.IsActive = true;
                        paidBillsCount++;
                    }
                    else
                    {
                        bill.IsPaid = false; // Partially paid
                    }
                    remainingCollection -= amountToPay;
                    await _unitOfWork.BillMasterRepository.UpdateAsync(bill);
                    // Add to CashBook / BankBook (Log the amount actually paid this time)
                    if (req.TransactionType == TransactionType.Cash)
                    {
                        var cashBookDebit = new CashBook
                        {
                            BillMasterId = bill.Id,
                            TransactionDate = DateTime.Now,
                            Debit = amountToPay,
                            Credit = 0,
                            Balance = 0,
                            AccountNo = "Cash",
                            VoucherNo = voucherNo,
                            Particulars = req.Particulars ?? ""
                        };
                        await _unitOfWork.CashBookRepository.AddAsync(cashBookDebit);

                        var cashBookCredit = new CashBook
                        {
                            BillMasterId = bill.Id,
                            TransactionDate = DateTime.Now,
                            Debit = 0,
                            Credit = amountToPay,
                            Balance = 0,
                            AccountNo = stdCID,
                            VoucherNo = voucherNo,
                            Particulars = "Bill Collection - " + (req.Particulars ?? "")
                        };
                        await _unitOfWork.CashBookRepository.AddAsync(cashBookCredit);
                    }
                    else if (req.TransactionType == TransactionType.Bank || req.TransactionType == TransactionType.Bkash)
                    {
                        var bankBookDebit = new BankBook
                        {
                            BillMasterId = bill.Id,
                            TransactionDate = DateTime.Now,
                            Debit = amountToPay,
                            Credit = 0,
                            Balance = 0,
                            BankName = req.BankName ?? "Bkash",
                            AccountNo = req.AccountNo ?? "",
                            TransactionNo = req.TransactionNo ?? "",
                            TransactionType = req.TransactionType,
                            VoucherNo = req.TrxId!,
                            Particulars = req.Particulars ?? ""
                        };
                        await _unitOfWork.BankBookRepository.AddAsync(bankBookDebit);

                        var bankBookCredit = new BankBook
                        {
                            BillMasterId = bill.Id,
                            TransactionDate = DateTime.Now,
                            Debit = 0,
                            Credit = amountToPay,
                            Balance = 0,
                            BankName = req.BankName ?? "Bkash",
                            AccountNo = stdCID,
                            TransactionNo = req.TransactionNo ?? "",
                            TransactionType = req.TransactionType,
                            VoucherNo = req.TrxId!,
                            Particulars = "Bill Collection - " + (req.Particulars ?? "")
                        };
                        await _unitOfWork.BankBookRepository.AddAsync(bankBookCredit);
                    }

                    else if (!bill.IsPaid)
                    {
                        // Just update the voucher number so it's included in the receipt as unpaid
                        await _unitOfWork.BillMasterRepository.UpdateAsync(bill);
                    }
                }

                //await _unitOfWork.CommitAsync(cancellationToken);
                var Returnbills = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                  .Include(x => x.Admission)
                  .ThenInclude(x => x.Student)
                  .Include(x => x.Details)
                  .Where(x => req.BillMasterIds.Contains(x.Id)) // Check IsPaid instead of IsActive
                  .OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth)
                  .ToListAsync(cancellationToken);

                try
                {
                    var student = Returnbills.FirstOrDefault()!.Admission.Student;

                    string studentName = student.StdCID!;
                    decimal paidAmount = request.Request.CollectionAmount;

                    string message = $"The bill for student {studentName} has been paid successfully. Paid Amount: ৳{paidAmount:N2}.";

                    var smsPayload = new
                    {
                        apikey = _configuration["SmsSettings:ApiKey"],
                        secretkey = _configuration["SmsSettings:SecretKey"],
                        callerID = _configuration["SmsSettings:CallerID"],
                        toUser = student.StudentPhone,
                        messageContent = message
                    };

                    using var httpClient = new System.Net.Http.HttpClient();
                    var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(smsPayload), System.Text.Encoding.UTF8, "application/json");
                    // Note: Ensure the endpoint path (e.g. /api/v1/send or /smsapi) matches what Songbird Telecom expects
                    var response = await httpClient.PostAsync("http://sms.songbirdtelecom.com:8746/sendtext", content);


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



                var res = new MultiMonthBillCollectionResponse
                {
                    VoucherNo = voucherNo,
                    PaidBillsCount = paidBillsCount,
                    TotalAmount = Returnbills.Sum(x => x.TotalAmount),
                    PaidAmount = req.CollectionAmount,
                    DueAmount = Returnbills.Sum(x => x.TotalAmount)-Returnbills.Sum(x => x.CollectionAmount),
                    StCID = Returnbills.FirstOrDefault()!.Admission.Student.StdCID,
                    Message = "Successfully paid bills",
                    TrxId = req.TrxId

                };

                return Result.Success(res, $"Successfully paid bills.");

            }
            else
            {
                return Result.Fail(StatusCodes.Status404NotFound, $"bill not found.");
            }



        }
        catch (Exception ex)
        {
            return Result.Fail<MultiMonthBillCollectionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
