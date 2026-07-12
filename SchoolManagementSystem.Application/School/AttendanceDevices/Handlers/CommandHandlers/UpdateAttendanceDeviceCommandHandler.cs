using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AttendanceDevices.Commands;
using SchoolManagementSystem.Application.School.AttendanceDevices.Models;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.CommandHandlers;

public class UpdateAttendanceDeviceCommandHandler : IHttpRequestHandler<UpdateAttendanceDeviceCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateAttendanceDeviceCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateAttendanceDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.AttendanceDevice.Id == Guid.Empty) return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status406NotAcceptable);
            
            var entity = await _unitOfWork.AttendanceDeviceRepository.GetSingleNoneDeletedAsync(x => x.Id == request.AttendanceDevice.Id);
            if (entity is null) return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status404NotFound);
            
            entity.DeviceNo = request.AttendanceDevice.DeviceNo;
            entity.CardNo = request.AttendanceDevice.CardNo;
            entity.DtPunchDate = request.AttendanceDevice.DtPunchDate;
            entity.DtPunchTime = request.AttendanceDevice.DtPunchTime;
            entity.InOut = request.AttendanceDevice.InOut;
            entity.IsActive = request.AttendanceDevice.IsActive;
            
            await _unitOfWork.AttendanceDeviceRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AttendanceDeviceResponse>(), "AttendanceDevice " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
