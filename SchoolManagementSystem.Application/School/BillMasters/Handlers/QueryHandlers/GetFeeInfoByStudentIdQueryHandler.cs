using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetFeeInfoByStudentIdQueryHandler : IHttpRequestHandler<GetFeeInfoByStudentIdQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeeInfoByStudentIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFeeInfoByStudentIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all bills for this student
            var bills = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Include(x => x.Admission)
                .Include(x => x.Details).ThenInclude(d => d.FeeHead)
                .Where(x => x.Admission.StudentId == request.StudentId)
                .OrderBy(x => x.BillYear).ThenBy(x => x.BillMonth)
                .ToListAsync(cancellationToken);

            var response = new FeeInfoResponse();

            if (bills.Any())
            {
                // Generate History
                response.History = bills.Select(x => new FeeHistory
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.BillMonth),
                    Amount = $"Tk {x.TotalAmount:N0}",
                    Status = x.IsPaid ? "Paid" : "Due"
                }).ToList();

                // Generate Summary from the distinct fee heads across bills
                var distinctFees = bills
                    .SelectMany(b => b.Details)
                    .Where(d => !d.IsDeleted && d.FeeHead != null)
                    .GroupBy(d => d.FeeHead.FeeHeadName)
                    .Select(g => new FeeSummary
                    {
                        Title = g.Key,
                        Amount = $"Tk {g.First().Amount:N0}"
                    })
                    .ToList();

                response.Summary = distinctFees;
            }

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<FeeInfoResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
