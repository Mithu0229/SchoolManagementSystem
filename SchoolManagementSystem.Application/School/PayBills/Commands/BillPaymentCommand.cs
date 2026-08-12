using MediatR;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.Application.School.PayBills.Commands;

public class BillPaymentCommand : IRequest<BillPaymentResponse>
{
    public BillPaymentRequest Request { get; set; } = new();
}
