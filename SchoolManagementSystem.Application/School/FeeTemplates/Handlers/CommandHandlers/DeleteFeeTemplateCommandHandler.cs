using SchoolManagementSystem.Application.School.FeeTemplates.Commands;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Handlers.CommandHandlers;

public class DeleteFeeTemplateCommandHandler : IHttpRequestHandler<DeleteFeeTemplateCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteFeeTemplateCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteFeeTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.FeeTemplateRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.FeeTemplateRepository.InstantDeleteWithDeactivate(entity);
            await _unitOfWork.FeeTemplateRepository.ReplaceManyAsync<FeeTemplateDetail>(x => x.FeeTemplateId == request.id, new List<FeeTemplateDetail>());
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
