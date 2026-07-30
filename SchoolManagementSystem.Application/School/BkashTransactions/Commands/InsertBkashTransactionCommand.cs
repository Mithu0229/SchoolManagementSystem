using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Commands;

public class InsertBkashTransactionCommand : IHttpRequest
{
    public BkashTransactionRequest BkashTransaction { get; set; }
}
