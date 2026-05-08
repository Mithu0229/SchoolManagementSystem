namespace SchoolManagementSystem.Application.School.Branches.Queries;

public record GetBranchListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
