using SchoolManagementSystem.Application.School.AcademicClasses.Models;
using SchoolManagementSystem.Application.School.AcademicClasses.Queries;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.QueryHandlers;

public class GetAcademicClassByIdQueryHandler : IHttpRequestHandler<GetAcademicClassByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicClassByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicClassByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<AcademicClassResponse>(StatusCodes.Status406NotAcceptable);
            }

            var result = await _unitOfWork.AcademicClassRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null)
            {
                return Result.Fail<AcademicClassResponse>(StatusCodes.Status404NotFound);
            }

            var response = result.Adapt<AcademicClassResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicClassResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
