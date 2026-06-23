using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Commands;

public class InsertBillMasterCommand : IHttpRequest
{
    public ProcessBillRequest ProcessBill { get; set; }
}
