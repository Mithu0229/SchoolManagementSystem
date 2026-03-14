using SchoolManagementSystem.Application.School.Students.Commands;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class DeleteStudentInfoCommandHandler : IHttpRequestHandler<DeleteStudentInfoCommand>
{
    private IUnitOfWork _unitOfWork;

    public DeleteStudentInfoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteStudentInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var studentInfo = await _unitOfWork.StudentInfoRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (studentInfo == null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound, "Student not found.");
            }

            var result = await _unitOfWork.StudentInfoRepository.InstantDelete(studentInfo);

            if (result)
            {
                return Result.Success<string>("Successfully Deleted");
            }
            else
            {
                return Result.Fail<string>("Failed to Delete");
            }
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, "An unexpected error occurred while deleting the Student.", ex.Message);
        }
    }
}
