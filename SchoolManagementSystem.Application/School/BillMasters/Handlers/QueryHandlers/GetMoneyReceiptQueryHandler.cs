using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using SchoolManagementSystem.Domain.Enums;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetMoneyReceiptQueryHandler : IHttpRequestHandler<GetMoneyReceiptQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMoneyReceiptQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    
    public async Task<IResult> Handle(GetMoneyReceiptQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status406NotAcceptable);

            var rawData = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Include(x => x.Admission).ThenInclude(a => a.Student).ThenInclude(s => s.GuardianInfo)
                .Include(x => x.Admission).ThenInclude(a => a.Class)
                .Include(x => x.Details).ThenInclude(d => d.FeeHead)
                .Where(x => x.Id == request.Id)
                .Select(x => new
                {
                    BillMasterId = x.Id,
                    FullName = x.Admission.Student.FullName,
                    StdCID = x.Admission.Student.StdCID,
                    StudentPhone = x.Admission.Student.StudentPhone,
                    FatherMobile = x.Admission.Student.GuardianInfo != null ? x.Admission.Student.GuardianInfo.FatherMobile : null,
                    ClassName = x.Admission.Class.ClassName,
                    TotalAmount = x.TotalAmount,
                    BillMonth = x.BillMonth,
                    BillYear = x.BillYear,
                    VoucherNo = x.VoucherNo,
                    PaymentDate = x.PaymentDate,
                    IsPaid = x.IsPaid,
                    CollectionAmount = x.CollectionAmount,
                    TransactionType = x.TransactionType,
                    Details = x.Details.Where(d => !d.IsDeleted).Select(d => new 
                    {
                        AccountHead = d.FeeHead.FeeHeadName,
                        Amount = d.Amount
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
                
            if (rawData is null) return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status404NotFound);

            // Fetch payment entries recorded in CashBook and BankBook for this bill
            var cashEntries = await _unitOfWork.CashBookRepository.GetAllNoneDeleted(false, true)
                .Where(c => c.BillMasterId == request.Id && c.Debit > 0)
                .OrderBy(c => c.TransactionDate)
                .ToListAsync(cancellationToken);

            var bankEntries = await _unitOfWork.BankBookRepository.GetAllNoneDeleted(false, true)
                .Where(b => b.BillMasterId == request.Id && b.Debit > 0)
                .OrderBy(b => b.TransactionDate)
                .ToListAsync(cancellationToken);

            decimal cashAmount = cashEntries.Sum(c => c.Debit);
            decimal bkashAmount = bankEntries.Where(b => b.TransactionType == TransactionType.Bkash).Sum(b => b.Debit);
            decimal bankAmount = bankEntries.Where(b => b.TransactionType == TransactionType.Bank).Sum(b => b.Debit);
            decimal totalPaidAmount = cashAmount + bkashAmount + bankAmount;

            // Fallback for bills where direct IsPaid or CollectionAmount was recorded without CashBook/BankBook rows
            if (totalPaidAmount == 0)
            {
                if (rawData.CollectionAmount > 0)
                {
                    totalPaidAmount = rawData.CollectionAmount;
                    if (rawData.TransactionType == TransactionType.Cash) cashAmount = totalPaidAmount;
                    else if (rawData.TransactionType == TransactionType.Bkash) bkashAmount = totalPaidAmount;
                    else if (rawData.TransactionType == TransactionType.Bank) bankAmount = totalPaidAmount;
                    else cashAmount = totalPaidAmount;
                }
                else if (rawData.IsPaid && rawData.TotalAmount > 0)
                {
                    totalPaidAmount = rawData.TotalAmount;
                    if (rawData.TransactionType == TransactionType.Cash) cashAmount = totalPaidAmount;
                    else if (rawData.TransactionType == TransactionType.Bkash) bkashAmount = totalPaidAmount;
                    else if (rawData.TransactionType == TransactionType.Bank) bankAmount = totalPaidAmount;
                    else cashAmount = totalPaidAmount;
                }
            }

            decimal totalDueAmount = Math.Max(0m, rawData.TotalAmount - totalPaidAmount);
            string paymentStatus = rawData.TotalAmount == 0
                ? "Paid"
                : (totalPaidAmount <= 0 ? "Unpaid" : (totalDueAmount <= 0 ? "Paid" : "Partial"));

            var monthNames = new[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string monthName = rawData.BillMonth >= 1 && rawData.BillMonth <= 12
                ? $"{monthNames[rawData.BillMonth]} {rawData.BillYear}"
                : $"{rawData.BillMonth}/{rawData.BillYear}";

            DateTime? latestTrxDate = null;
            if (cashEntries.Any() || bankEntries.Any())
            {
                var latestCash = cashEntries.Any() ? cashEntries.Max(c => c.TransactionDate) : (DateTime?)null;
                var latestBank = bankEntries.Any() ? bankEntries.Max(b => b.TransactionDate) : (DateTime?)null;
                latestTrxDate = latestCash.HasValue && latestBank.HasValue
                    ? (latestCash > latestBank ? latestCash : latestBank)
                    : (latestCash ?? latestBank);
            }

            var receiptDate = (latestTrxDate ?? rawData.PaymentDate ?? DateTime.Now).ToString("dd MMM yyyy, hh:mm tt");
            var invId = !string.IsNullOrWhiteSpace(rawData.VoucherNo)
                ? rawData.VoucherNo
                : (cashEntries.FirstOrDefault()?.VoucherNo ?? bankEntries.FirstOrDefault()?.VoucherNo ?? $"MR-{rawData.BillYear}{rawData.BillMonth:D2}");

            var paymentRows = new List<MoneyReceiptPaymentRowResponse>();
            int pSn = 1;
            foreach (var c in cashEntries)
            {
                paymentRows.Add(new MoneyReceiptPaymentRowResponse
                {
                    SN = pSn++,
                    Source = "CashBook",
                    PaymentMethod = "Cash",
                    AccountOrBank = string.IsNullOrWhiteSpace(c.AccountNo) ? "Cash Counter" : c.AccountNo,
                    TrxNoOrVoucher = !string.IsNullOrWhiteSpace(c.VoucherNo) ? c.VoucherNo : invId,
                    TransactionDate = c.TransactionDate.ToString("dd MMM yyyy hh:mm tt"),
                    Particulars = !string.IsNullOrWhiteSpace(c.Particulars) ? c.Particulars : "Cash Payment",
                    Amount = c.Debit
                });
            }

            foreach (var b in bankEntries)
            {
                paymentRows.Add(new MoneyReceiptPaymentRowResponse
                {
                    SN = pSn++,
                    Source = "BankBook",
                    PaymentMethod = b.TransactionType == TransactionType.Bkash ? "bKash" : "Bank",
                    AccountOrBank = !string.IsNullOrWhiteSpace(b.BankName) ? b.BankName : (b.TransactionType == TransactionType.Bkash ? "bKash" : "Bank"),
                    TrxNoOrVoucher = !string.IsNullOrWhiteSpace(b.TransactionNo) ? b.TransactionNo : (!string.IsNullOrWhiteSpace(b.VoucherNo) ? b.VoucherNo : invId),
                    TransactionDate = b.TransactionDate.ToString("dd MMM yyyy hh:mm tt"),
                    Particulars = !string.IsNullOrWhiteSpace(b.Particulars) ? b.Particulars : $"{b.TransactionType} Payment",
                    Amount = b.Debit
                });
            }

            // Fallback payment row if no cashbook/bankbook records were found
            if (!paymentRows.Any() && totalPaidAmount > 0)
            {
                string method = rawData.TransactionType == TransactionType.Bkash ? "bKash" : (rawData.TransactionType == TransactionType.Bank ? "Bank" : "Cash");
                paymentRows.Add(new MoneyReceiptPaymentRowResponse
                {
                    SN = 1,
                    Source = method == "Cash" ? "CashBook" : "BankBook",
                    PaymentMethod = method,
                    AccountOrBank = method == "Cash" ? "Cash Counter" : method,
                    TrxNoOrVoucher = invId,
                    TransactionDate = receiptDate,
                    Particulars = "Bill Collection",
                    Amount = totalPaidAmount
                });
            }
            
            var response = new MoneyReceiptResponse
            {
                BillMasterId = rawData.BillMasterId,
                StudentName = rawData.FullName ?? "N/A",
                StudentID = rawData.StdCID ?? "N/A",
                Phone = rawData.StudentPhone ?? (rawData.FatherMobile ?? "N/A"),
                ClassName = rawData.ClassName ?? "N/A",
                Date = receiptDate,
                ManualMR = 0,
                InvID = invId,
                TotalAmount = rawData.TotalAmount,
                TotalBilledAmount = rawData.TotalAmount,
                CashAmount = cashAmount,
                BankAmount = bankAmount,
                BkashAmount = bkashAmount,
                TotalPaidAmount = totalPaidAmount,
                TotalDueAmount = totalDueAmount,
                BillMonth = rawData.BillMonth,
                BillYear = rawData.BillYear,
                MonthName = monthName,
                VoucherNo = rawData.VoucherNo ?? invId,
                PaymentStatus = paymentStatus,
                Inword = NumberToWords(totalPaidAmount > 0 ? totalPaidAmount : rawData.TotalAmount),
                Details = rawData.Details.Select((d, index) => new MoneyReceiptDetailResponse
                {
                    SN = index + 1,
                    AccountHead = d.AccountHead ?? "N/A",
                    Month = monthName,
                    TransID = invId,
                    Amount = d.Amount
                }).ToList(),
                PaymentRows = paymentRows
            };
            
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }

    private static string NumberToWords(decimal doubleNumber)
    {
        var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
        var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)} Taka";
        return beforeFloatingPointWord;
    }

    private static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
}
