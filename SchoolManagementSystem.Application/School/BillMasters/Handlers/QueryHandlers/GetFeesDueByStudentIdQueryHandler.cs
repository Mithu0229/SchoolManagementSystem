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

    public async Task<IResult> Handle(GetFeesDueByStudentIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Include(x => x.Admission)
                .Where(x => x.Admission.StudentId == request.StudentId
                            && x.BillMonth == request.Month
                            && x.BillYear == request.Year);

            var items = await query.Select(x => new FeesDueResponse
            {
                Installment = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.BillMonth),
                Date = "-", // You can format a specific date if applicable
                Amount = $"{x.TotalAmount:N2}/-"
            }).ToListAsync(cancellationToken);

            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<FeesDueResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
