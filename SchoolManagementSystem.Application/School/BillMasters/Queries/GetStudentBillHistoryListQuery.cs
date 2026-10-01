using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetStudentBillHistoryListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; } = new PagedRequest();
}

public record GetStudentBillHistorySummaryQuery : IHttpRequest
{
    public PagedRequest? PagedRequest { get; set; }
}
