using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Shifts.Commands;
using SchoolManagementSystem.Application.School.Shifts.Models;

namespace SchoolManagementSystem.Application.School.Shifts.Handlers.CommandHandlers;

public class InsertShiftCommandHandler : IHttpRequestHandler<InsertShiftCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertShiftCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<ShiftResponse>(StatusCodes.Status406NotAcceptable);
            request.Shift.ShiftName = request.Shift.ShiftName.Trim();
            var duplicate = await _unitOfWork.ShiftRepository.GetAllNoneDeleted().Where(x => x.ShiftName.ToLower() == request.Shift.ShiftName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Shift already exists!");
            var entity = request.Shift.Adapt<Shift>();
            await _unitOfWork.ShiftRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<ShiftResponse>(), "Shift " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<ShiftResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
