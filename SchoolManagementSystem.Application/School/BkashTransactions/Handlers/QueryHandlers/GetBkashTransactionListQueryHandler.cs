using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.BkashTransactions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.QueryHandlers;

public class GetBkashTransactionListQueryHandler : IHttpRequestHandler<GetBkashTransactionListQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBkashTransactionListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBkashTransactionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.BkashTransactionRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.FromNumber.ToLower().Contains(search) ||
                                         x.TransactionType.ToLower().Contains(search));
            }

            var totalRecord = await query.CountAsync(cancellationToken);

            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) 
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);

            var items = await query.ToListAsync(cancellationToken);
            var mappedItems = items.Adapt<List<BkashTransactionResponse>>();

            return Result.Success(new PagedResult<BkashTransactionResponse> { Items = mappedItems, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<BkashTransactionResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
