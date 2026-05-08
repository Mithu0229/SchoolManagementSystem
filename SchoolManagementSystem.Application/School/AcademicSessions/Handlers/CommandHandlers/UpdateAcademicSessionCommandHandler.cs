using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicSessions.Commands;
using SchoolManagementSystem.Application.School.AcademicSessions.Models;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.CommandHandlers;

public class UpdateAcademicSessionCommandHandler : IHttpRequestHandler<UpdateAcademicSessionCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateAcademicSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateAcademicSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.AcademicSession.Id == Guid.Empty)
            {
                return Result.Fail<AcademicSessionResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.AcademicSession.SessionName = request.AcademicSession.SessionName.Trim();
            var entity = await _unitOfWork.AcademicSessionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.AcademicSession.Id);
            if (entity is null)
            {
                return Result.Fail<AcademicSessionResponse>(StatusCodes.Status404NotFound);
            }

            var duplicate = await _unitOfWork.AcademicSessionRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.AcademicSession.Id && x.SessionName.ToLower() == request.AcademicSession.SessionName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Academic session already exists!");
            }

            entity.SessionName = request.AcademicSession.SessionName;
            entity.FromDate = request.AcademicSession.FromDate;
            entity.ToDate = request.AcademicSession.ToDate;
            entity.IsCurrent = request.AcademicSession.IsCurrent;
            entity.IsActive = request.AcademicSession.IsActive;

            await _unitOfWork.AcademicSessionRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<AcademicSessionResponse>();
            return Result.Success(response, "AcademicSession " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicSessionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
