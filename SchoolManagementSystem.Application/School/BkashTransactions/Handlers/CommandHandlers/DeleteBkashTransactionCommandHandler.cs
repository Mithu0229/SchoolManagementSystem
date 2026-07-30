using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.CommandHandlers;

public class DeleteBkashTransactionCommandHandler : IHttpRequestHandler<DeleteBkashTransactionCommand>
{
    private IUnitOfWork _unitOfWork;

    public DeleteBkashTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteBkashTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.BkashTransactionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity == null) return Result.Fail(StatusCodes.Status404NotFound, AlertMessage.NotFoundMessage);

            await _unitOfWork.BkashTransactionRepository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(StatusCodes.Status200OK, "BkashTransaction " + AlertMessage.DeleteMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
