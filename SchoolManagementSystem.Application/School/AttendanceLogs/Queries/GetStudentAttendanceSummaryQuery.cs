using MediatR;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.AttendanceLogs.Queries;

public record GetStudentAttendanceSummaryQuery : IRequest<IResult<List<StudentAttendanceDayResponse>>>
{
    public Guid StudentId { get; set; }
}

public class StudentAttendanceDayResponse
{
    public DateTime Date { get; set; }
    public DateTime FirstPunch { get; set; }
    public DateTime LastPunch { get; set; }
    public int PunchCount { get; set; }
}
