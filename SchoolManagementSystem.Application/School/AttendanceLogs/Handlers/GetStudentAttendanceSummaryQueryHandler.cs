using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AttendanceLogs.Queries;

namespace SchoolManagementSystem.Application.School.AttendanceLogs.Handlers;

public class GetStudentAttendanceSummaryQueryHandler
    : IRequestHandler<GetStudentAttendanceSummaryQuery, IResult<List<StudentAttendanceDayResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentAttendanceSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<List<StudentAttendanceDayResponse>>> Handle(
        GetStudentAttendanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var studentCode = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(true, true)
            .Where(x => x.Id == request.StudentId)
            .Select(x => x.StdCID)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(studentCode))
        {
            return Result.Success(new List<StudentAttendanceDayResponse>());
        }

        studentCode = studentCode.Trim();

        // AttendanceLogs.Student_code == current student's code, only logs already sent (IsSent = true)
        var punches = await _unitOfWork.AttendanceLogRepository.GetAll(true)
            .Where(x => x.Student_code == studentCode && x.IsSent)
            .Select(x => x.Punch_time)
            .ToListAsync(cancellationToken);

        var days = punches
            .GroupBy(p => p.Date)
            .Select(g => new StudentAttendanceDayResponse
            {
                Date = g.Key,
                FirstPunch = g.Min(),
                LastPunch = g.Max(),
                PunchCount = g.Count()
            })
            .OrderByDescending(d => d.Date)
            .ToList();

        return Result.Success(days);
    }
}
