using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Shifts.Queries;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.QueryHandlers;

public class GetShiftDropdownQueryHandler : IHttpRequestHandler<GetShiftDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetShiftDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetShiftDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.ShiftRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.ShiftName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
