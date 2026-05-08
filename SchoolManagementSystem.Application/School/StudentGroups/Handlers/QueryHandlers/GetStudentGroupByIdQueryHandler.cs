using SchoolManagementSystem.Application.School.StudentGroups.Models;
using SchoolManagementSystem.Application.School.StudentGroups.Queries;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.QueryHandlers;

public class GetStudentGroupByIdQueryHandler : IHttpRequestHandler<GetStudentGroupByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentGroupByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentGroupByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<StudentGroupResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.StudentGroupRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<StudentGroupResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<StudentGroupResponse>());
        }
        catch (Exception ex) { return Result.Fail<StudentGroupResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
