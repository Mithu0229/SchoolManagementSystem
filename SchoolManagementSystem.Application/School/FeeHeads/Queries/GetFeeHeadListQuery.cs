namespace SchoolManagementSystem.Application.School.FeeHeads.Queries;

public record GetFeeHeadListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
