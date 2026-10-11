using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Accounting.Models;

namespace SchoolManagementSystem.Application.School.Accounting.Queries;

public class GetBankBookReportQuery : IRequest<List<BankBookReportDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetBankBookReportQueryHandler : IRequestHandler<GetBankBookReportQuery, List<BankBookReportDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBankBookReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<BankBookReportDto>> Handle(GetBankBookReportQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.BankBookRepository.GetAllNoneDeleted().AsQueryable();

        if (request.StartDate.HasValue)
            query = query.Where(x => x.TransactionDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(x => x.TransactionDate <= request.EndDate.Value);

        var data = await query
            .OrderBy(x => x.TransactionDate)
            .Select(x => new BankBookReportDto
            {
                Id = x.Id,
                TransactionDate = x.TransactionDate,
                Particulars = x.Particulars,
                VoucherNo = x.VoucherNo,
                BankName = x.BankName,
                AccountNo = x.AccountNo,
                TransactionNo = x.TransactionNo,
                Debit = x.Debit,
                Credit = x.Credit,
                Balance = x.Balance
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}
