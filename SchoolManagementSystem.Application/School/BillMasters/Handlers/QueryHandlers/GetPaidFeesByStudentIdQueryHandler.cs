using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetPaidFeesByStudentIdQueryHandler : IHttpRequestHandler<GetPaidFeesByStudentIdQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaidFeesByStudentIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetPaidFeesByStudentIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Include(x => x.Admission)
                .Where(x => x.Admission.StudentId == request.StudentId );//&& x.IsActive == request.IsActive

            var items = await query.Select(x => new PaidFeeResponse
            {
                Id = x.Id,
                Date = (x.ModifiedDate ?? x.CreatedDate).ToString("dd/MMM/yy"),
                Amount = x.TotalAmount.ToString("0.##"),
                Slip = "Print"
            }).ToListAsync(cancellationToken);

            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<PaidFeeResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
