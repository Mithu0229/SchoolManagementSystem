using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AttendanceDevices.Queries;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.QueryHandlers;

public class GetAttendanceDeviceDropdownQueryHandler : IHttpRequestHandler<GetAttendanceDeviceDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAttendanceDeviceDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAttendanceDeviceDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.AttendanceDeviceRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.DeviceNo + " - " + x.CardNo })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
