using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.CommandHandlers;

public class UpdateBkashTransactionCommandHandler : IHttpRequestHandler<UpdateBkashTransactionCommand>
{
    private IUnitOfWork _unitOfWork;

    public UpdateBkashTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateBkashTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.BkashTransactionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity is null) return Result.Fail(StatusCodes.Status404NotFound, AlertMessage.NotFoundMessage);

            request.BkashTransaction.Adapt(entity);
            await _unitOfWork.BkashTransactionRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(entity.Adapt<BkashTransactionResponse>(), "BkashTransaction " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
