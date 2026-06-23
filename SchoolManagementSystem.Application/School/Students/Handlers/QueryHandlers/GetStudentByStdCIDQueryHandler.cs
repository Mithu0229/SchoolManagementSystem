using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;

public class GetStudentByStdCIDQueryHandler : IHttpRequestHandler<GetStudentByStdCIDQuery>
{
    private IUnitOfWork _unitOfWork;

    public GetStudentByStdCIDQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentByStdCIDQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.StdCID))
            {
                return Result.Fail<StudentByStdCIDResponse>(StatusCodes.Status400BadRequest, "StdCID is required.");
            }

            var student = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted()
                .Where(x => x.StdCID == request.StdCID)
                .Select(x => new StudentByStdCIDResponse
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    StdCID = x.StdCID

                })
                .FirstOrDefaultAsync(cancellationToken);

            if (student == null)
            {
                return Result.Fail<StudentByStdCIDResponse>(StatusCodes.Status404NotFound, "Student not found.");
            }

            return Result.Success(student);
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentByStdCIDResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
