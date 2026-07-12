using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AttendanceDevices.Commands;
using SchoolManagementSystem.Application.School.AttendanceDevices.Models;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.CommandHandlers;

public class InsertAttendanceDeviceCommandHandler : IHttpRequestHandler<InsertAttendanceDeviceCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertAttendanceDeviceCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertAttendanceDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status406NotAcceptable);
            
            var entity = request.AttendanceDevice.Adapt<AttendanceDevice>();
            await _unitOfWork.AttendanceDeviceRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AttendanceDeviceResponse>(), "AttendanceDevice " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
