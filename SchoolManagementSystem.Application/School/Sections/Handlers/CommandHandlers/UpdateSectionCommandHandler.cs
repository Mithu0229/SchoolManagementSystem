using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Sections.Commands;
using SchoolManagementSystem.Application.School.Sections.Models;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.CommandHandlers;

public class UpdateSectionCommandHandler : IHttpRequestHandler<UpdateSectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateSectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Section.Id == Guid.Empty) return Result.Fail<SectionResponse>(StatusCodes.Status406NotAcceptable);
            request.Section.SectionName = request.Section.SectionName.Trim();
            var entity = await _unitOfWork.SectionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Section.Id);
            if (entity is null) return Result.Fail<SectionResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.SectionRepository.GetAllNoneDeleted().Where(x => x.Id != request.Section.Id && x.SectionName.ToLower() == request.Section.SectionName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Section already exists!");
            entity.SectionName = request.Section.SectionName;
            entity.Remarks = request.Section.Remarks;
            entity.IsActive = request.Section.IsActive;
            await _unitOfWork.SectionRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<SectionResponse>(), "Section " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<SectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
