using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Shifts.Commands;
using SchoolManagementSystem.Application.School.Shifts.Models;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.CommandHandlers;

public class UpdateShiftCommandHandler : IHttpRequestHandler<UpdateShiftCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateShiftCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Shift.Id == Guid.Empty) return Result.Fail<ShiftResponse>(StatusCodes.Status406NotAcceptable);
            request.Shift.ShiftName = request.Shift.ShiftName.Trim();
            var entity = await _unitOfWork.ShiftRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Shift.Id);
            if (entity is null) return Result.Fail<ShiftResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.ShiftRepository.GetAllNoneDeleted().Where(x => x.Id != request.Shift.Id && x.ShiftName.ToLower() == request.Shift.ShiftName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Shift already exists!");
            entity.ShiftName = request.Shift.ShiftName;
            entity.IsActive = request.Shift.IsActive;
            await _unitOfWork.ShiftRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<ShiftResponse>(), "Shift " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<ShiftResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
