using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeTemplates.Commands;
using SchoolManagementSystem.Application.School.FeeTemplates.Models;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Handlers.CommandHandlers;

public class UpdateFeeTemplateCommandHandler : IHttpRequestHandler<UpdateFeeTemplateCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateFeeTemplateCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateFeeTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.FeeTemplate.Id == Guid.Empty) return Result.Fail<FeeTemplateResponse>(StatusCodes.Status406NotAcceptable);
            request.FeeTemplate.TemplateName = request.FeeTemplate.TemplateName.Trim();
            var entity = await _unitOfWork.FeeTemplateRepository.GetSingleNoneDeletedAsync(x => x.Id == request.FeeTemplate.Id);
            if (entity is null) return Result.Fail<FeeTemplateResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.FeeTemplate.Id && x.TemplateName.ToLower() == request.FeeTemplate.TemplateName.ToLower() && x.ClassId == request.FeeTemplate.ClassId && x.GroupId == request.FeeTemplate.GroupId && x.ShiftId == request.FeeTemplate.ShiftId)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Fee template already exists!");
            entity.TemplateName = request.FeeTemplate.TemplateName;
            entity.ClassId = request.FeeTemplate.ClassId;
            entity.GroupId = request.FeeTemplate.GroupId;
            entity.ShiftId = request.FeeTemplate.ShiftId;
            entity.IsActive = request.FeeTemplate.IsActive;
            await _unitOfWork.FeeTemplateRepository.UpdateAsync(entity);
            var details = request.FeeTemplate.Details.Select(x => new FeeTemplateDetail { FeeTemplateId = entity.Id, FeeHeadId = x.FeeHeadId, Amount = x.Amount }).ToList();
            await _unitOfWork.FeeTemplateRepository.ReplaceManyAsync<FeeTemplateDetail>(x => x.FeeTemplateId == entity.Id, details);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<FeeTemplateResponse>(), "FeeTemplate " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeTemplateResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
