using SchoolManagementSystem.Application.School.Institutes.Models;
using SchoolManagementSystem.Application.School.Institutes.Queries;

namespace SchoolManagementSystem.Application.School.Institutes.Handlers.QueryHandlers;

public class GetInstituteByIdQueryHandler : IHttpRequestHandler<GetInstituteByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetInstituteByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetInstituteByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<InstituteResponse>(StatusCodes.Status406NotAcceptable);
            }

            var result = await _unitOfWork.InstituteRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null)
            {
                return Result.Fail<InstituteResponse>(StatusCodes.Status404NotFound);
            }

            var response = result.Adapt<InstituteResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<InstituteResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
