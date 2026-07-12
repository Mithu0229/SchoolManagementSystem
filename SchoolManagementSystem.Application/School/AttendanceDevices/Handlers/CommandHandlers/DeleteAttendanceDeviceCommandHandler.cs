using SchoolManagementSystem.Application.School.AttendanceDevices.Commands;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.CommandHandlers;

public class DeleteAttendanceDeviceCommandHandler : IHttpRequestHandler<DeleteAttendanceDeviceCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteAttendanceDeviceCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteAttendanceDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.AttendanceDeviceRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.AttendanceDeviceRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
