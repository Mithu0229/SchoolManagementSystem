using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class UpdateStudentUserCommandHandler : IHttpRequestHandler<UpdateStudentUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateStudentUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Request == null || request.Request.StudentId == Guid.Empty)
            {
                return Result.Fail<bool>(StatusCodes.Status400BadRequest);
            }

            var user = await _unitOfWork.UserRepository.GetAllNoneDeleted(false,true)
                .FirstOrDefaultAsync(u => u.StudentId == request.Request.StudentId, cancellationToken);

            if (user == null)
            {
                return Result.Fail<bool>(StatusCodes.Status404NotFound);
            }

            user.IsActive = request.Request.IsActive;

            if (!string.IsNullOrEmpty(request.Request.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Request.Password);
            }

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            return Result.Success(true, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return Result.Fail<bool>(StatusCodes.Status500InternalServerError);
        }
    }
}
