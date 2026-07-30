using Mapster;
using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.BkashTransactions.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.QueryHandlers;

public class GetBkashTransactionByIdQueryHandler : IHttpRequestHandler<GetBkashTransactionByIdQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBkashTransactionByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBkashTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.BkashTransactionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity == null) return Result.Fail(StatusCodes.Status404NotFound, AlertMessage.NotFoundMessage);

            return Result.Success(entity.Adapt<BkashTransactionResponse>());
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
