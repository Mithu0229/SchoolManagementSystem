using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class UpdateStudentUserCommandHandler : IHttpRequestHandler<UpdateStudentUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISmsService _smsService;

    public UpdateStudentUserCommandHandler(IUnitOfWork unitOfWork, ISmsService smsService)
    {
        _unitOfWork = unitOfWork;
        _smsService = smsService;
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

            if (request.Request.SendSms)
            {
                var student = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(false,true).FirstOrDefaultAsync(x=>x.Id==request.Request.StudentId);
                if (student != null && !string.IsNullOrEmpty(student.StudentPhone))
                {
                    string passwordToSend = string.IsNullOrEmpty(request.Request.Password) ? "your current password" : request.Request.Password;
                    string loginUrl = "http://edugates.net/login"; // placeholder, configure as needed
                    string message = $"Dear Student, your username is {student.StdCID} and password is {passwordToSend}. Login at: {loginUrl}";
                    
                    await _smsService.SendSmsAsync(student.StudentPhone, message);
                }
            }

            return Result.Success(true, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return Result.Fail<bool>(StatusCodes.Status500InternalServerError);
        }
    }
}
