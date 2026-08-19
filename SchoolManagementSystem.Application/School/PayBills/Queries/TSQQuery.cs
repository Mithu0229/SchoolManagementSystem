using MediatR;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.Application.School.PayBills.Queries;

public class TSQQuery 
{
    public TSQRequest Request { get; set; } = new();
}
