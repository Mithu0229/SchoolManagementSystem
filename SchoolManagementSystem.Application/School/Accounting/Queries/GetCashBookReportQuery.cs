using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Accounting.Models;

namespace SchoolManagementSystem.Application.School.Accounting.Queries;

public class GetCashBookReportQuery : IRequest<List<CashBookReportDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetCashBookReportQueryHandler : IRequestHandler<GetCashBookReportQuery, List<CashBookReportDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCashBookReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CashBookReportDto>> Handle(GetCashBookReportQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.CashBookRepository.GetAllNoneDeleted().AsQueryable();

        if (request.StartDate.HasValue)
            query = query.Where(x => x.TransactionDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(x => x.TransactionDate <= request.EndDate.Value);

        var data = await query
            .OrderBy(x => x.TransactionDate)
            .Select(x => new CashBookReportDto
            {
                Id = x.Id,
                TransactionDate = x.TransactionDate,
                Particulars = x.Particulars,
                VoucherNo = x.VoucherNo,
                AccountNo = x.AccountNo,
                Debit = x.Debit,
                Credit = x.Credit,
                Balance = x.Balance
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}
