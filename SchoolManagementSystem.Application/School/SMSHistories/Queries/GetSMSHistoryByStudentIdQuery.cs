using MediatR;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.SMSHistories.Queries;

public record GetSMSHistoryByStudentIdQuery : IRequest<IResult<List<SMSHistoryResponse>>>
{
    public Guid StudentId { get; set; }
}

public class SMSHistoryResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Message { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedDate { get; set; }
}
