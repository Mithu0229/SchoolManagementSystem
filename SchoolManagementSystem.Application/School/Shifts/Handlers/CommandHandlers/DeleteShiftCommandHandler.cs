using SchoolManagementSystem.Application.School.Shifts.Commands;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.CommandHandlers;

public class DeleteShiftCommandHandler : IHttpRequestHandler<DeleteShiftCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteShiftCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.ShiftRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.ShiftRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
