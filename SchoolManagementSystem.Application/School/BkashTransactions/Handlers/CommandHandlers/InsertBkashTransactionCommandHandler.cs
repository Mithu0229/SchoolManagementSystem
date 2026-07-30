using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Domain.Entities.Schools;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.CommandHandlers;

public class InsertBkashTransactionCommandHandler : IHttpRequestHandler<InsertBkashTransactionCommand>
{
    private IUnitOfWork _unitOfWork;

    public InsertBkashTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertBkashTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable);

            var entity = request.BkashTransaction.Adapt<BkashTransaction>();
            await _unitOfWork.BkashTransactionRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(entity.Adapt<BkashTransactionResponse>(), "BkashTransaction " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<BkashTransactionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
