using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetFeesDueByStudentIdQueryHandler : IHttpRequestHandler<GetFeesDueByStudentIdQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeesDueByStudentIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(
     GetFeesDueByStudentIdQuery request,
     CancellationToken cancellationToken)
    {
        try
        {
            // Create date using requested year/month and current day
            var currentDay = DateTime.Now.Day;

            // Prevent invalid date, e.g. February 30
            var day = Math.Min(
                currentDay,
                DateTime.DaysInMonth(request.Year, request.Month)
            );

            var installmentDate = new DateTime(
                request.Year,
                request.Month,
                day
            );

            // Get student's unpaid bills up to requested month
            var bills = await _unitOfWork.BillMasterRepository
                .GetAllNoneDeleted(false, true)
                .Where(x =>
                    x.Admission != null &&
                    x.Admission.Student != null &&
                    x.Admission.StudentId == request.StudentId &&
                    !x.IsPaid &&
                    !x.IsActive &&
                    (
                        x.BillYear < request.Year ||
                        (
                            x.BillYear == request.Year &&
                            x.BillMonth <= request.Month
                        )
                    )
                )
                .ToListAsync(cancellationToken);

            // Calculate total due amount
            var totalAmount = bills.Sum(x => x.TotalAmount);

            // Get month name
            var installment = CultureInfo.CurrentCulture
                .DateTimeFormat
                .GetMonthName(request.Month);

            // Create response
            var response = new List<FeesDueResponse>
        {
            new FeesDueResponse
            {
                Amount = totalAmount.ToString(),
                Date = installmentDate.ToString("dd/MM/yyyy"),
                Installment = installment
            }
        };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<FeesDueResponse>>(
                StatusCodes.Status500InternalServerError,
                ex.Message
            );
        }
    }
}