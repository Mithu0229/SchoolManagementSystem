using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class UpdateStudentInfoOnlyCommandHandler : IHttpRequestHandler<UpdateStudentInfoOnlyCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateStudentInfoOnlyCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(UpdateStudentInfoOnlyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<StudentInfoResponse>(StatusCodes.Status406NotAcceptable);
            }

            var studentInfo = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted()
                .FirstOrDefaultAsync(s => s.Id == request.StudentInfo.Id, cancellationToken);

            if (studentInfo == null)
            {
                return Result.Fail<StudentInfoResponse>(StatusCodes.Status404NotFound, "Student not found.");
            }

            studentInfo.FullName = request.StudentInfo.FullName;
            studentInfo.Gender = request.StudentInfo.Gender;
            studentInfo.DateOfBirth = request.StudentInfo.DateOfBirth;
            studentInfo.PlaceOfBirth = request.StudentInfo.PlaceOfBirth;
            studentInfo.Nationality = request.StudentInfo.Nationality;
            studentInfo.Religion = request.StudentInfo.Religion;
            studentInfo.BloodGroup = request.StudentInfo.BloodGroup;
            studentInfo.BirthCertificateNo = request.StudentInfo.BirthCertificateNo;
            studentInfo.ApplicationForClass = request.StudentInfo.ApplicationForClass;
            studentInfo.AcademicYear = request.StudentInfo.AcademicYear;
            studentInfo.LastSchool = request.StudentInfo.LastSchool;
            studentInfo.LastClassAttendedResult = request.StudentInfo.LastClassAttendedResult;
            studentInfo.IsDisability = request.StudentInfo.IsDisability?.ToString();
            studentInfo.Disability = request.StudentInfo.Disability;
            studentInfo.SpecialCare = request.StudentInfo.SpecialCare;
            studentInfo.PresentAddress = request.StudentInfo.PresentAddress;
            studentInfo.PermanentAddress = request.StudentInfo.PermanentAddress;
            studentInfo.StudentPhone = request.StudentInfo.StudentPhone;
            studentInfo.StudentEmail = request.StudentInfo.StudentEmail;

            if (!string.IsNullOrWhiteSpace(request.StudentInfo.ImagePath))
            {
                studentInfo.ImagePath = request.StudentInfo.ImagePath;
            }

            await _unitOfWork.StudentInfoRepository.UpdateAsync(studentInfo);
            await _unitOfWork.CommitAsync();

            var response = studentInfo.Adapt<StudentInfoResponse>();
            return Result.Success(response, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentInfoResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
