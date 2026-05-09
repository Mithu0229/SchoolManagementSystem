using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeTemplates.Commands;
using SchoolManagementSystem.Application.School.FeeTemplates.Models;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Handlers.CommandHandlers;

public class InsertFeeTemplateCommandHandler : IHttpRequestHandler<InsertFeeTemplateCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertFeeTemplateCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertFeeTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<FeeTemplateResponse>(StatusCodes.Status406NotAcceptable);
            request.FeeTemplate.TemplateName = request.FeeTemplate.TemplateName.Trim();
            var duplicate = await _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted()
                .Where(x => x.TemplateName.ToLower() == request.FeeTemplate.TemplateName.ToLower() && x.ClassId == request.FeeTemplate.ClassId && x.GroupId == request.FeeTemplate.GroupId && x.ShiftId == request.FeeTemplate.ShiftId)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Fee template already exists!");
            var entity = request.FeeTemplate.Adapt<FeeTemplate>();
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            entity.Details = new List<FeeTemplateDetail>();
            await _unitOfWork.FeeTemplateRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            if (request.FeeTemplate.Details.Count > 0)
            {
                var details = request.FeeTemplate.Details.Select(x => new FeeTemplateDetail { FeeTemplateId = entity.Id, FeeHeadId = x.FeeHeadId, Amount = x.Amount }).ToList();
                await _unitOfWork.FeeTemplateRepository.ReplaceManyAsync<FeeTemplateDetail>(x => x.FeeTemplateId == entity.Id, details);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            return Result.Success(entity.Adapt<FeeTemplateResponse>(), "FeeTemplate " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeTemplateResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
