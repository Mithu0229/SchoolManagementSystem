using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetBillMasterDropdownQueryHandler : IHttpRequestHandler<GetBillMasterDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBillMasterDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBillMasterDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.Admission.RollNo + " - " + x.BillMonth + "/" + x.BillYear })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
