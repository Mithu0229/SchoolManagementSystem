using SchoolManagementSystem.Application.School.Shifts.Models;
using SchoolManagementSystem.Application.School.Shifts.Queries;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.QueryHandlers;

public class GetShiftByIdQueryHandler : IHttpRequestHandler<GetShiftByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetShiftByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<ShiftResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.ShiftRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<ShiftResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<ShiftResponse>());
        }
        catch (Exception ex) { return Result.Fail<ShiftResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
