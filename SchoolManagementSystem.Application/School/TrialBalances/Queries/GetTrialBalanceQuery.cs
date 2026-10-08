using MediatR;
using SchoolManagementSystem.Application.School.TrialBalances.Models;
using System;

namespace SchoolManagementSystem.Application.School.TrialBalances.Queries;

public class GetTrialBalanceQuery : IRequest<TrialBalanceReportDto>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
