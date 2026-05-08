using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Sections.Commands;
using SchoolManagementSystem.Application.School.Sections.Models;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.CommandHandlers;

public class InsertSectionCommandHandler : IHttpRequestHandler<InsertSectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertSectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<SectionResponse>(StatusCodes.Status406NotAcceptable);
            request.Section.SectionName = request.Section.SectionName.Trim();
            var duplicate = await _unitOfWork.SectionRepository.GetAllNoneDeleted().Where(x => x.SectionName.ToLower() == request.Section.SectionName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Section already exists!");
            var entity = request.Section.Adapt<Section>();
            await _unitOfWork.SectionRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<SectionResponse>(), "Section " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<SectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
