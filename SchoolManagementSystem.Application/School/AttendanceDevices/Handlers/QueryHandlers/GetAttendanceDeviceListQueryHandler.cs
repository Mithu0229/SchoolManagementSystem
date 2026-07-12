using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AttendanceDevices.Models;
using SchoolManagementSystem.Application.School.AttendanceDevices.Queries;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Handlers.QueryHandlers;

public class GetAttendanceDeviceListQueryHandler : IHttpRequestHandler<GetAttendanceDeviceListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAttendanceDeviceListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetAttendanceDeviceListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.AttendanceDeviceRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.DeviceNo.ToLower().Contains(search) || x.CardNo.ToLower().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            
            var items = await query.Select(x => new AttendanceDeviceResponse { 
                Id = x.Id, 
                DeviceNo = x.DeviceNo, 
                CardNo = x.CardNo, 
                DtPunchDate = x.DtPunchDate, 
                DtPunchTime = x.DtPunchTime, 
                InOut = x.InOut, 
                IsActive = x.IsActive 
            }).ToListAsync(cancellationToken);
            
            return Result.Success(new PagedResult<AttendanceDeviceResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<AttendanceDeviceResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
