using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Application.School.PayBills.Queries;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Commands;

public class InsertBkashTransactionCommand : IHttpRequest
{
    public BkashTransactionRequest BkashTransaction { get; set; }
}

public class TSQQueryCommand : IHttpRequest
{
    public TSQRequest Request { get; set; }
}
public class CheckBillCommand : IHttpRequest
{
    public CheckBillRequest Request { get; set; }
}
