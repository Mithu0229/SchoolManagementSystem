using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Commands;

public class MultiMonthBillCollectionCommand : IHttpRequest 
{
    public MultiMonthBillCollectionRequest Request { get; set; } = new MultiMonthBillCollectionRequest();
}
