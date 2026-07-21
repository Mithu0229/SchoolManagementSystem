using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

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
                    Details = x.Details.Where(d => !d.IsDeleted).Select(d => new 
                    {
                        AccountHead = d.FeeHead.FeeHeadName,
                        Amount = d.Amount
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
                
            if (rawData is null) return Result.Fail<MoneyReceiptResponse>(StatusCodes.Status404NotFound);
            
            var response = new MoneyReceiptResponse
            {
                BillMasterId = rawData.BillMasterId,
                StudentName = rawData.FullName ?? "N/A",
                StudentID = rawData.StdCID,
                Phone = rawData.StudentPhone ?? (rawData.FatherMobile ?? "N/A"),
                ClassName = rawData.ClassName,
                Date = DateTime.UtcNow.ToString("MM/dd/yyyy"),
                ManualMR = 0,
                InvID = "23",
                TotalAmount = rawData.TotalAmount,
                Inword = NumberToWords(rawData.TotalAmount),
                Details = rawData.Details.Select((d, index) => new MoneyReceiptDetailResponse
                {
                    SN = index + 1,
                    AccountHead = d.AccountHead,
                    Month = "",
                    TransID = "std" + (2040 + index),
                    Amount = d.Amount
                }).ToList()
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
