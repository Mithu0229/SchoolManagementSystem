using MediatR;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.Application.School.PayBills.Queries;

public class CheckBillQuery : IRequest<CheckBillResponse>
{
    public CheckBillRequest Request { get; set; } = new();
}
