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
        var fromDate = request.FromDate.Date;
        var toDate = request.ToDate.Date.AddDays(1).AddTicks(-1);

        // 1. Bank Book
        var bankBooks = await _unitOfWork.BankBookRepository.GetAllNoneDeleted().ToListAsync(cancellationToken);
        var bankOpeningDebit = bankBooks.Where(x => x.TransactionDate < fromDate).Sum(x => x.Debit);
        var bankOpeningCredit = bankBooks.Where(x => x.TransactionDate < fromDate).Sum(x => x.Credit);
        
        var bankTransDebit = bankBooks.Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).Sum(x => x.Debit);
        var bankTransCredit = bankBooks.Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).Sum(x => x.Credit);

        var bankOpeningBalance = bankOpeningDebit - bankOpeningCredit;
        var bankNetTrans = bankTransDebit - bankTransCredit;
        var bankClosingBalance = bankOpeningBalance + bankNetTrans;

        result.Items.Add(new TrialBalanceItemDto
        {
            Particulars = "Bank Book",
            OpeningBalance = bankOpeningBalance,
            DebitTrans = bankTransDebit,
            CreditTrans = bankTransCredit,
            NetTrans = bankNetTrans,
            Dr = bankClosingBalance >= 0 ? bankClosingBalance : 0,
            Cr = bankClosingBalance < 0 ? Math.Abs(bankClosingBalance) : 0
        });

        // 2. Cash Book
        var cashBooks = await _unitOfWork.CashBookRepository.GetAllNoneDeleted().ToListAsync(cancellationToken);
        var cashOpeningDebit = cashBooks.Where(x => x.TransactionDate < fromDate).Sum(x => x.Debit);
        var cashOpeningCredit = cashBooks.Where(x => x.TransactionDate < fromDate).Sum(x => x.Credit);

        var cashTransDebit = cashBooks.Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).Sum(x => x.Debit);
        var cashTransCredit = cashBooks.Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).Sum(x => x.Credit);

        var cashOpeningBalance = cashOpeningDebit - cashOpeningCredit;
        var cashNetTrans = cashTransDebit - cashTransCredit;
        var cashClosingBalance = cashOpeningBalance + cashNetTrans;

        result.Items.Add(new TrialBalanceItemDto
        {
            Particulars = "Cash Book",
            OpeningBalance = cashOpeningBalance,
            DebitTrans = cashTransDebit,
            CreditTrans = cashTransCredit,
            NetTrans = cashNetTrans,
            Dr = cashClosingBalance >= 0 ? cashClosingBalance : 0,
            Cr = cashClosingBalance < 0 ? Math.Abs(cashClosingBalance) : 0
        });

        // 3. Account Receivable (Bill Master)
        var billMasters = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted().ToListAsync(cancellationToken);
        
        // Debit is TotalAmount, Credit is CollectionAmount
        var arOpeningDebit = billMasters.Where(x => x.CreatedDate < fromDate).Sum(x => x.TotalAmount);
        var arOpeningCredit = billMasters.Where(x => x.CreatedDate < fromDate).Sum(x => x.CollectionAmount);

        var arTransDebit = billMasters.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).Sum(x => x.TotalAmount);
        var arTransCredit = billMasters.Where(x => x.PaymentDate >= fromDate && x.PaymentDate <= toDate).Sum(x => x.CollectionAmount);

        var arOpeningBalance = arOpeningDebit - arOpeningCredit;
        var arNetTrans = arTransDebit - arTransCredit;
        var arClosingBalance = arOpeningBalance + arNetTrans;

        result.Items.Add(new TrialBalanceItemDto
        {
            Particulars = "Account Receivable",
            OpeningBalance = arOpeningBalance,
            DebitTrans = arTransDebit,
            CreditTrans = arTransCredit,
            NetTrans = arNetTrans,
            Dr = arClosingBalance >= 0 ? arClosingBalance : 0,
            Cr = arClosingBalance < 0 ? Math.Abs(arClosingBalance) : 0
        });

        return result;
    }
}
