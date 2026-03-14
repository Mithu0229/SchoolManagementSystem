using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;


public class GetStudentInfoByIdQueryHandler : IHttpRequestHandler<GetStudentInfoByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public GetStudentInfoByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(GetStudentInfoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id != Guid.Empty)
            {
                var studentInfo = await _unitOfWork.StudentInfoRepository
                    .GetAllNoneDeleted(false, true)
                    .Include(x => x.GuardianInfo)
                    .Include(x => x.LocalGuardianInfo)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (studentInfo == null)
                {
                    return Result.Fail<StudentInfoResponse>(StatusCodes.Status404NotFound, "Student not found.");
                }

                var response = new StudentInfoResponse
                {
                    Id = studentInfo.Id,
                    FullName = studentInfo.FullName,
                    Gender = studentInfo.Gender,
                    DateOfBirth = studentInfo.DateOfBirth,
                    PlaceOfBirth = studentInfo.PlaceOfBirth,
                    Nationality = studentInfo.Nationality,
                    Religion = studentInfo.Religion,
                    BloodGroup = studentInfo.BloodGroup,
                    BirthCertificateNo = studentInfo.BirthCertificateNo,
                    ApplicationForClass = studentInfo.ApplicationForClass,
                    AcademicYear = studentInfo.AcademicYear,
                    LastSchool = studentInfo.LastSchool,
                    LastClassAttendedResult = studentInfo.LastClassAttendedResult,
                    IsDisability = studentInfo.IsDisability,
                    Disability = studentInfo.Disability,
                    SpecialCare = studentInfo.SpecialCare,
                    PresentAddress = studentInfo.PresentAddress,
                    PermanentAddress = studentInfo.PermanentAddress,
                    StudentPhone = studentInfo.StudentPhone,
                    StudentEmail = studentInfo.StudentEmail,
                    ImagePath = studentInfo.ImagePath,
                    FatherName = studentInfo.GuardianInfo?.FatherName,
                    MotherName = studentInfo.GuardianInfo?.MotherName,
                    Name = studentInfo.LocalGuardianInfo?.Name,
                    GuardianInfo = studentInfo.GuardianInfo.Adapt<GuardianInfoResponse>(),
                    LocalGuardianInfo = studentInfo.LocalGuardianInfo.Adapt<LocalGuardianInfoResponse>()
                };

                return Result.Success(response);
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentInfoResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

