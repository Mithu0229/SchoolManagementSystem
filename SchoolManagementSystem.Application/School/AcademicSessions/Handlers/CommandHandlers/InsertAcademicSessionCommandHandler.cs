using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicSessions.Commands;
using SchoolManagementSystem.Application.School.AcademicSessions.Models;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.CommandHandlers;

public class InsertAcademicSessionCommandHandler : IHttpRequestHandler<InsertAcademicSessionCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertAcademicSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertAcademicSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<AcademicSessionResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.AcademicSession.SessionName = request.AcademicSession.SessionName.Trim();
            var duplicate = await _unitOfWork.AcademicSessionRepository.GetAllNoneDeleted()
                .Where(x => x.SessionName.ToLower() == request.AcademicSession.SessionName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Academic session already exists!");
            }

            var entity = request.AcademicSession.Adapt<AcademicSession>();
            await _unitOfWork.AcademicSessionRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<AcademicSessionResponse>();
            return Result.Success(response, "AcademicSession " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicSessionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
