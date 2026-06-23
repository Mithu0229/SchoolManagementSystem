namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetBillMasterListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
