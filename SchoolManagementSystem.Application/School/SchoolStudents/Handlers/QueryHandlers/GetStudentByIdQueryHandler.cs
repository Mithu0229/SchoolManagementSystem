using SchoolManagementSystem.Application.School.SchoolStudents.Models;
using SchoolManagementSystem.Application.School.SchoolStudents.Queries;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Handlers.QueryHandlers;

public class GetStudentByIdQueryHandler : IHttpRequestHandler<GetStudentByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<StudentResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.StudentRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<StudentResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<StudentResponse>());
        }
        catch (Exception ex) { return Result.Fail<StudentResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
