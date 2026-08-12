using MediatR;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.Application.School.PayBills.Queries;

public class TSQQuery : IRequest<TSQResponse>
{
    public TSQRequest Request { get; set; } = new();
}
