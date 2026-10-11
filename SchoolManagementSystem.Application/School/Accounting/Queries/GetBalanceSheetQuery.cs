using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Accounting.Models;
using System.Linq;

namespace SchoolManagementSystem.Application.School.Accounting.Queries;

public class GetBalanceSheetQuery : IRequest<BalanceSheetDto>
{
    public DateTime AsOfDate { get; set; }
}

public class GetBalanceSheetQueryHandler : IRequestHandler<GetBalanceSheetQuery, BalanceSheetDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBalanceSheetQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BalanceSheetDto> Handle(GetBalanceSheetQuery request, CancellationToken cancellationToken)
    {
        var asOfDate = request.AsOfDate.Date.AddDays(1).AddTicks(-1);
        var result = new BalanceSheetDto { AsOfDate = request.AsOfDate.Date };

        // 1. Assets
        // 1.1 Bank Balance
        var bankBooks = await _unitOfWork.BankBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate <= asOfDate)
            .ToListAsync(cancellationToken);
        
        var bankBalance = bankBooks.Sum(x => x.Debit) - bankBooks.Sum(x => x.Credit);
        if (bankBalance != 0)
        {
            result.Assets.Add(new BalanceSheetItemDto { Particulars = "Bank Balance", Amount = bankBalance });
        }

        // 1.2 Cash Balance
        var cashBooks = await _unitOfWork.CashBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate <= asOfDate)
            .ToListAsync(cancellationToken);

        var cashBalance = cashBooks.Sum(x => x.Debit) - cashBooks.Sum(x => x.Credit);
        if (cashBalance != 0)
        {
            result.Assets.Add(new BalanceSheetItemDto { Particulars = "Cash Balance", Amount = cashBalance });
        }

        // 1.3 Accounts Receivable (Unpaid Bills)
        var billMasters = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
            .Where(x => x.CreatedDate <= asOfDate)
            .ToListAsync(cancellationToken);

        var accountsReceivable = billMasters.Sum(x => x.TotalAmount) - billMasters.Where(x => x.PaymentDate <= asOfDate).Sum(x => x.CollectionAmount);
        if (accountsReceivable != 0)
        {
            result.Assets.Add(new BalanceSheetItemDto { Particulars = "Accounts Receivable", Amount = accountsReceivable });
        }

        result.TotalAssets = result.Assets.Sum(x => x.Amount);

        // 2. Liabilities and Equity
        // 2.1 Equity (Retained Earnings = Net Income)
        // Calculating Net Income up to AsOfDate
        var totalBankRevenue = bankBooks.Sum(x => x.Debit);
        var totalCashRevenue = cashBooks.Sum(x => x.Debit);
        var totalRevenue = totalBankRevenue + totalCashRevenue;

        var totalBankExpense = bankBooks.Sum(x => x.Credit);
        var totalCashExpense = cashBooks.Sum(x => x.Credit);
        var totalExpense = totalBankExpense + totalCashExpense;

        var netIncome = totalRevenue - totalExpense;

        result.LiabilitiesAndEquity.Add(new BalanceSheetItemDto { Particulars = "Retained Earnings", Amount = netIncome });

        result.TotalLiabilitiesAndEquity = result.LiabilitiesAndEquity.Sum(x => x.Amount);

        return result;
    }
}
