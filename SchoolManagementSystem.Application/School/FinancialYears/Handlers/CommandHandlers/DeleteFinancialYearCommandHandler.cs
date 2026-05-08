using SchoolManagementSystem.Application.School.FinancialYears.Commands;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.CommandHandlers;

public class DeleteFinancialYearCommandHandler : IHttpRequestHandler<DeleteFinancialYearCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteFinancialYearCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteFinancialYearCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var entity = await _unitOfWork.FinancialYearRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            await _unitOfWork.FinancialYearRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
