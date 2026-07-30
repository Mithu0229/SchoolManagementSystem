using System;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Commands;

public class DeleteBkashTransactionCommand : IHttpRequest
{
    public Guid Id { get; set; }
}
