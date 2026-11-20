using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Commands;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class UpdateStudentPhotoCommandHandler : IHttpRequestHandler<UpdateStudentPhotoCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentPhotoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateStudentPhotoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var studentInfo = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted()
                .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

            if (studentInfo == null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound, "Student not found.");
            }

            studentInfo.ImagePath = request.ImagePath;

            await _unitOfWork.StudentInfoRepository.UpdateAsync(studentInfo);
            await _unitOfWork.CommitAsync();

            return Result.Success(request.ImagePath, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
