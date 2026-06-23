namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetBillMasterByIdQuery(Guid Id) : IHttpRequest;
