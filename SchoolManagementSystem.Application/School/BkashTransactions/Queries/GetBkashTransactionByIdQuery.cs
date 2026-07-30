using System;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Queries;

public record GetBkashTransactionByIdQuery : IHttpRequest
{
    public Guid Id { get; set; }
}
