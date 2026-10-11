using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Accounting.Models;
using System.Linq;

namespace SchoolManagementSystem.Application.School.Accounting.Queries;

public class GetIncomeStatementQuery : IRequest<IncomeStatementDto>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class GetIncomeStatementQueryHandler : IRequestHandler<GetIncomeStatementQuery, IncomeStatementDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIncomeStatementQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IncomeStatementDto> Handle(GetIncomeStatementQuery request, CancellationToken cancellationToken)
    {
        var fromDate = request.FromDate.Date;
        var toDate = request.ToDate.Date.AddDays(1).AddTicks(-1);

        var result = new IncomeStatementDto
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        // Bank Book
        var bankBooks = await _unitOfWork.BankBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate)
            .ToListAsync(cancellationToken);

        // Cash Book
        var cashBooks = await _unitOfWork.CashBookRepository.GetAllNoneDeleted()
            .Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate)
            .ToListAsync(cancellationToken);

        // Revenue (Debits in Bank/Cash)
        var totalBankRevenue = bankBooks.Sum(x => x.Debit);
        var totalCashRevenue = cashBooks.Sum(x => x.Debit);

        if (totalBankRevenue > 0)
        {
            result.Revenues.Add(new IncomeStatementItemDto { Particulars = "Revenue via Bank", Amount = totalBankRevenue });
        }
        if (totalCashRevenue > 0)
        {
            result.Revenues.Add(new IncomeStatementItemDto { Particulars = "Revenue via Cash", Amount = totalCashRevenue });
        }

        result.TotalRevenue = totalBankRevenue + totalCashRevenue;

        // Expenses (Credits in Bank/Cash)
        var totalBankExpense = bankBooks.Sum(x => x.Credit);
        var totalCashExpense = cashBooks.Sum(x => x.Credit);

        if (totalBankExpense > 0)
        {
            result.Expenses.Add(new IncomeStatementItemDto { Particulars = "Expenses via Bank", Amount = totalBankExpense });
        }
        if (totalCashExpense > 0)
        {
            result.Expenses.Add(new IncomeStatementItemDto { Particulars = "Expenses via Cash", Amount = totalCashExpense });
        }

        result.TotalExpense = totalBankExpense + totalCashExpense;

        return result;
    }
}
