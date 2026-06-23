namespace SchoolManagementSystem.Application.School.BillMasters.Commands;

public record DeleteBillMasterCommand(Guid id) : IHttpRequest;
