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
