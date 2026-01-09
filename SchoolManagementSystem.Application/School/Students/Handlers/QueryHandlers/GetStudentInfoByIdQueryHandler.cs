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
                var response = await _unitOfWork.StudentInfoRepository
    .GetAllNoneDeleted(false, true)
    .Where(x => x.Id == request.Id)
    .Select(x => new StudentInfoResponse
    {
        Id = x.Id,
        FullName = x.FullName,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth,
        PlaceOfBirth = x.PlaceOfBirth,
        Nationality = x.Nationality,
        Religion = x.Religion,
        BloodGroup = x.BloodGroup,
        BirthCertificateNo = x.BirthCertificateNo,
        ApplicationForClass = x.ApplicationForClass,
        AcademicYear = x.AcademicYear,
        LastSchool = x.LastSchool,
        LastClassAttendedResult = x.LastClassAttendedResult,
        IsDisability = x.IsDisability,
        Disability = x.Disability,
        SpecialCare = x.SpecialCare,
        PresentAddress = x.PresentAddress,
        PermanentAddress = x.PermanentAddress,
        StudentPhone = x.StudentPhone,
        StudentEmail = x.StudentEmail,

        GuardianInfo = x.GuardianInfo,
        LocalGuardianInfo = x.LocalGuardianInfo
    })
    .FirstOrDefaultAsync();
                // var result = await _unitOfWork.StudentInfoRepository
                //.GetAllNoneDeleted(false, true)
                //.Where(x => x.Id == request.Id)
                //.Include(x => x.GuardianInfo)
                // .Include(x => x.LocalGuardianInfo)
                //.FirstOrDefaultAsync();

                // if (result != null)
                // {
                //     var response = result.Select(x => new StudentInfoResponse
                //     {

                //     })
                //         //result.Adapt<StudentInfoResponse>();

                     return Result.Success(response);
                // }
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentInfoResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

