using SchoolManagementSystem.Application.GS.Users.Commands;
using SchoolManagementSystem.Application.GS.Users.Models;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Domain.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class InsertStudentInfoCommandHandler: IHttpRequestHandler<InsertStudentInfoCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public InsertStudentInfoCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(InsertStudentInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<StudentInfoResponse>(StatusCodes.Status406NotAcceptable);
            }
            if(request.StudentInfo != null)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.StudentInfo.FullName,
                    LastName = request.StudentInfo.FullName,
                    Email = request.StudentInfo.FullName,
                    Password = request.StudentInfo.FullName,
                    UserType = Domain.Enums.UserTypes.Student,
                    PhoneNumber = request.StudentInfo.StudentPhone,
                    IsActive = true

                };
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.StudentInfo!.FullName);
                await _unitOfWork.UserRepository.AddAsync(user);
            }

            var studentInfo = request.StudentInfo.Adapt<StudentInfo>();
            await _unitOfWork.StudentInfoRepository.AddAsync(studentInfo);
            await _unitOfWork.CommitAsync();
            var response = studentInfo.Adapt<StudentInfoResponse>();
            return Result.Success(response, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
