using SchoolManagementSystem.Application.School.AttendanceDevices.Models;
using SchoolManagementSystem.Application.School.AttendanceDevices.Queries;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.QueryHandlers;

public class GetAttendanceDeviceByIdQueryHandler : IHttpRequestHandler<GetAttendanceDeviceByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAttendanceDeviceByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetAttendanceDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.AttendanceDeviceRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<AttendanceDeviceResponse>());
        }
        catch (Exception ex) { return Result.Fail<AttendanceDeviceResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
