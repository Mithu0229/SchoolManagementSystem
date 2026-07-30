using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Queries;

public record GetBkashTransactionListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
