using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetStudentPaidBillReportQueryHandler : IHttpRequestHandler<GetStudentPaidBillReportQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentPaidBillReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentPaidBillReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rawDataList = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false,true)
                .Include(x => x.Admission).ThenInclude(a => a.Student).ThenInclude(s => s.GuardianInfo)
                .Include(x => x.Admission).ThenInclude(a => a.Class)
                .Include(x => x.Details).ThenInclude(d => d.FeeHead)
                .Where(x => x.Admission.StudentId == request.StudentId && x.IsPaid)
                .ToListAsync(cancellationToken);

            if (!rawDataList.Any()) return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status404NotFound, "No paid bills found for this student.");

            // Use the first record to extract common student info
            var firstRecord = rawDataList.First();
            var totalAmount = rawDataList.Sum(x => x.TotalAmount);

            var response = new MoneyReceiptResponse
            {
                BillMasterId = Guid.Empty, // Or a specific ID if needed, but it's an aggregate report
                StudentName = firstRecord.Admission.Student.FullName ?? "N/A",
                StudentID = firstRecord.Admission.Student.StdCID,
                Phone = firstRecord.Admission.Student.StudentPhone ?? (firstRecord.Admission.Student.GuardianInfo?.FatherMobile ?? "N/A"),
                ClassName = firstRecord.Admission.Class.ClassName,
                Date = DateTime.UtcNow.ToString("MM/dd/yyyy"),
                ManualMR = 0,
                InvID = "Aggregate",
                TotalAmount = totalAmount,
                Inword = NumberToWords(totalAmount)
            };

            var details = new List<MoneyReceiptDetailResponse>();
            int sn = 1;

            foreach (var bill in rawDataList.OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth))
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(bill.BillMonth);
                foreach (var detail in bill.Details.Where(d => !d.IsDeleted))
                {
                    details.Add(new MoneyReceiptDetailResponse
                    {
                        SN = sn++,
                        AccountHead = detail.FeeHead?.FeeHeadName ?? "Unknown",
                        Month = $"{monthName} {bill.BillYear}",
                        TransID = "std" + (2040 + sn), // Mock TransID based on existing logic
                        Amount = detail.Amount
                    });
                }
            }

            response.Details = details;

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private static string NumberToWords(decimal doubleNumber)
    {
        var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
        var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)} Taka";
        return beforeFloatingPointWord;
    }

    private static string NumberToWords(int number)
    {
        if (number == 0) return "zero";
        if (number < 0) return "minus " + NumberToWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0) { words += NumberToWords(number / 1000000) + " million "; number %= 1000000; }
        if ((number / 1000) > 0) { words += NumberToWords(number / 1000) + " thousand "; number %= 1000; }
        if ((number / 100) > 0) { words += NumberToWords(number / 100) + " hundred "; number %= 100; }
        if (number > 0)
        {
            if (words != "") words += "and ";
            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            if (number < 20) words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0) words += "-" + unitsMap[number % 10];
            }
        }
        return words;
    }
}
