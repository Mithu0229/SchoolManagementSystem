using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.TrialBalances.Models;
using SchoolManagementSystem.Application.School.TrialBalances.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.TrialBalances.Handlers.QueryHandlers;

public class GetTrialBalanceQueryHandler : IRequestHandler<GetTrialBalanceQuery, TrialBalanceReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTrialBalanceQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TrialBalanceReportDto> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken)
    {
        var result = new TrialBalanceReportDto();

        // Period = [fromDate, toDateExclusive). Opening = everything strictly before fromDate.
        var fromDate = request.FromDate.Date;
        var toDateExclusive = request.ToDate.Date.AddDays(1);

        // 1. Bank Book (by TransactionDate = payment date)
        var bank = await _unitOfWork.BankBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate < toDateExclusive)
            .GroupBy(x => 1)
            .Select(g => new
            {
                OpeningDebit = g.Where(x => x.TransactionDate < fromDate).Sum(x => (decimal?)x.Debit) ?? 0,
                OpeningCredit = g.Where(x => x.TransactionDate < fromDate).Sum(x => (decimal?)x.Credit) ?? 0,
                PeriodDebit = g.Where(x => x.TransactionDate >= fromDate).Sum(x => (decimal?)x.Debit) ?? 0,
                PeriodCredit = g.Where(x => x.TransactionDate >= fromDate).Sum(x => (decimal?)x.Credit) ?? 0
            })
            .FirstOrDefaultAsync(cancellationToken);

        result.Items.Add(BuildItem("Bank Book",
            // Payment handlers post a Dr (cash in) and a contra Cr of the same amount,
            // so only the Debit side is the real asset movement.
            bank?.OpeningDebit ?? 0, 0,
            bank?.PeriodDebit ?? 0, 0));

        // 2. Cash Book (by TransactionDate = payment date)
        var cash = await _unitOfWork.CashBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate < toDateExclusive)
            .GroupBy(x => 1)
            .Select(g => new
            {
                OpeningDebit = g.Where(x => x.TransactionDate < fromDate).Sum(x => (decimal?)x.Debit) ?? 0,
                OpeningCredit = g.Where(x => x.TransactionDate < fromDate).Sum(x => (decimal?)x.Credit) ?? 0,
                PeriodDebit = g.Where(x => x.TransactionDate >= fromDate).Sum(x => (decimal?)x.Debit) ?? 0,
                PeriodCredit = g.Where(x => x.TransactionDate >= fromDate).Sum(x => (decimal?)x.Credit) ?? 0
            })
            .FirstOrDefaultAsync(cancellationToken);

        result.Items.Add(BuildItem("Cash Book",
            cash?.OpeningDebit ?? 0, 0,
            cash?.PeriodDebit ?? 0, 0));

        // 3. Account Receivable (Bill Master)
        //    Debit  = bills raised   -> TotalAmount by CreatedDate
        //    Credit = bills collected -> CollectionAmount by PaymentDate
        var bills = _unitOfWork.BillMasterRepository.GetAllNoneDeleted();

        var arOpeningDebit = await bills
            .Where(x => x.CreatedDate < fromDate)
            .SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0;

        var arPeriodDebit = await bills
            .Where(x => x.CreatedDate >= fromDate && x.CreatedDate < toDateExclusive)
            .SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0;

        var arOpeningCredit = await bills
            .Where(x => x.PaymentDate != null && x.PaymentDate < fromDate)
            .SumAsync(x => (decimal?)x.CollectionAmount, cancellationToken) ?? 0;

        var arPeriodCredit = await bills
            .Where(x => x.PaymentDate != null && x.PaymentDate >= fromDate && x.PaymentDate < toDateExclusive)
            .SumAsync(x => (decimal?)x.CollectionAmount, cancellationToken) ?? 0;

        result.Items.Add(BuildItem("Account Receivable",
            arOpeningDebit, arOpeningCredit, arPeriodDebit, arPeriodCredit));

        // 4. Fee Income: credit side of every bill raised (balances the trial balance)
        result.Items.Add(BuildItem("Fee Income",
            0, arOpeningDebit,
            0, arPeriodDebit));

        return result;
    }

    private static TrialBalanceItemDto BuildItem(
        string particulars,
        decimal openingDebit, decimal openingCredit,
        decimal periodDebit, decimal periodCredit)
    {
        var opening = openingDebit - openingCredit;
        var net = periodDebit - periodCredit;
        var closing = opening + net;

        return new TrialBalanceItemDto
        {
            Particulars = particulars,
            OpeningBalance = opening,
            DebitTrans = periodDebit,
            CreditTrans = periodCredit,
            NetTrans = net,
            Dr = closing >= 0 ? closing : 0,
            Cr = closing < 0 ? Math.Abs(closing) : 0
        };
    }
}
