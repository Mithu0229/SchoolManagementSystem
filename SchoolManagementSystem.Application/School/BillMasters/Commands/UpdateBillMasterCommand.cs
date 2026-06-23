using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Commands;

public class UpdateBillMasterCommand : IHttpRequest
{
    public BillMasterRequest BillMaster { get; set; }
}
