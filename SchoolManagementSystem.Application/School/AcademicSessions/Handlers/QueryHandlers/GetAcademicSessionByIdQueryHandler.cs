using SchoolManagementSystem.Application.School.AcademicSessions.Models;
using SchoolManagementSystem.Application.School.AcademicSessions.Queries;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.QueryHandlers;

public class GetAcademicSessionByIdQueryHandler : IHttpRequestHandler<GetAcademicSessionByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicSessionByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicSessionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<AcademicSessionResponse>(StatusCodes.Status406NotAcceptable);
            }

            var result = await _unitOfWork.AcademicSessionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null)
            {
                return Result.Fail<AcademicSessionResponse>(StatusCodes.Status404NotFound);
            }

            var response = result.Adapt<AcademicSessionResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicSessionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
