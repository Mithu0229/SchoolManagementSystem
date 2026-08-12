namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetPaidBillListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
