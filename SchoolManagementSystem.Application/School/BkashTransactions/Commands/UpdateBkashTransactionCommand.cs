using System;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Commands;

public class UpdateBkashTransactionCommand : IHttpRequest
{
    public Guid Id { get; set; }
    public BkashTransactionRequest BkashTransaction { get; set; }
}
