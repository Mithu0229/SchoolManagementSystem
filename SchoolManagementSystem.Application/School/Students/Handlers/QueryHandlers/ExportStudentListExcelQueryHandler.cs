using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Excel;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;

public class ExportStudentListExcelQueryHandler : IHttpRequestHandler<ExportStudentListExcelQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportStudentListExcelQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(ExportStudentListExcelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Fetch active classes and create lookup dictionary for class name mapping
            var classEntities = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted(true, true)
                .Where(x => x.IsActive)
                .OrderBy(x => x.ClassName)
                .ToListAsync(cancellationToken);

            var classNames = classEntities.Select(x => x.ClassName).ToList();
            var classMapById = classEntities
                .GroupBy(x => x.Id.ToString(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().ClassName, StringComparer.OrdinalIgnoreCase);

            // 2. Query students with guardian details
            var query = _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(true, true)
                .Include(x => x.GuardianInfo)
                .Include(x => x.LocalGuardianInfo)
                .AsQueryable();

            if (!request.ExportAll)
            {
                if (request.StudentIds != null && request.StudentIds.Count > 0)
                {
                    query = query.Where(x => request.StudentIds.Contains(x.Id));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.ClassId))
                    {
                        var classId = request.ClassId.Trim();
                        query = query.Where(x => x.ApplicationForClass == classId);
                    }

                    if (!string.IsNullOrWhiteSpace(request.Search))
                    {
                        var search = request.Search.Trim().ToLower();
                        query = query.Where(x =>
                            (x.FullName != null && x.FullName.ToLower().Contains(search)) ||
                            (x.StdCID != null && x.StdCID.ToLower().Contains(search)) ||
                            (x.StudentPhone != null && x.StudentPhone.Contains(search)) ||
                            (x.StudentEmail != null && x.StudentEmail.ToLower().Contains(search)));
                    }
                }
            }

            var students = await query
                .OrderBy(x => x.StdCID)
                .ThenBy(x => x.FullName)
                .ToListAsync(cancellationToken);

            // 3. Map student entities to template row dictionary
            var rows = new List<Dictionary<string, object?>>();
            foreach (var s in students)
            {
                var className = s.ApplicationForClass ?? string.Empty;
                if (!string.IsNullOrEmpty(className) && classMapById.TryGetValue(className, out var resolvedName))
                {
                    className = resolvedName;
                }

                var genderStr = s.Gender switch
                {
                    1 => "Male",
                    2 => "Female",
                    3 => "Other",
                    _ => string.Empty
                };

                var row = new Dictionary<string, object?>
                {
                    [StudentExcelTemplate.StdCID] = s.StdCID,
                    [StudentExcelTemplate.FullName] = s.FullName,
                    [StudentExcelTemplate.Gender] = genderStr,
                    [StudentExcelTemplate.DateOfBirth] = s.DateOfBirth != default ? s.DateOfBirth.ToString("yyyy-MM-dd") : string.Empty,
                    [StudentExcelTemplate.PlaceOfBirth] = s.PlaceOfBirth,
                    [StudentExcelTemplate.Nationality] = s.Nationality,
                    [StudentExcelTemplate.Religion] = s.Religion,
                    [StudentExcelTemplate.BloodGroup] = s.BloodGroup,
                    [StudentExcelTemplate.BirthCertificateNo] = s.BirthCertificateNo,
                    [StudentExcelTemplate.ApplicationForClass] = className,
                    [StudentExcelTemplate.AcademicYear] = s.AcademicYear,
                    [StudentExcelTemplate.StudentPhone] = s.StudentPhone,
                    [StudentExcelTemplate.StudentEmail] = s.StudentEmail,
                    [StudentExcelTemplate.PresentAddress] = s.PresentAddress,
                    [StudentExcelTemplate.PermanentAddress] = s.PermanentAddress,
                    [StudentExcelTemplate.LastSchool] = s.LastSchool,
                    [StudentExcelTemplate.LastClassAttendedResult] = s.LastClassAttendedResult,
                    [StudentExcelTemplate.IsDisability] = s.IsDisability,
                    [StudentExcelTemplate.Disability] = s.Disability,
                    [StudentExcelTemplate.SpecialCare] = s.SpecialCare,

                    // Father
                    [StudentExcelTemplate.FatherName] = s.GuardianInfo?.FatherName,
                    [StudentExcelTemplate.FatherMobile] = s.GuardianInfo?.FatherMobile,
                    [StudentExcelTemplate.FatherOccupation] = s.GuardianInfo?.FatherOccupation,
                    [StudentExcelTemplate.FatherAcademicQualification] = s.GuardianInfo?.FatherAcademicQualification,
                    [StudentExcelTemplate.FatherNationalIdNo] = s.GuardianInfo?.FatherNationalIdNo,
                    [StudentExcelTemplate.FatherEmail] = s.GuardianInfo?.FatherEmail,
                    [StudentExcelTemplate.FatherTelephoneOffice] = s.GuardianInfo?.FatherTelephoneOffice,
                    [StudentExcelTemplate.FatherTelephoneResidence] = s.GuardianInfo?.FatherTelephoneResidence,

                    // Mother
                    [StudentExcelTemplate.MotherName] = s.GuardianInfo?.MotherName,
                    [StudentExcelTemplate.MotherMobile] = s.GuardianInfo?.MotherMobile,
                    [StudentExcelTemplate.MotherOccupation] = s.GuardianInfo?.MotherOccupation,
                    [StudentExcelTemplate.MotherAcademicQualification] = s.GuardianInfo?.MotherAcademicQualification,
                    [StudentExcelTemplate.MotherNationalIdNo] = s.GuardianInfo?.MotherNationalIdNo,
                    [StudentExcelTemplate.MotherEmail] = s.GuardianInfo?.MotherEmail,
                    [StudentExcelTemplate.MotherTelephoneOffice] = s.GuardianInfo?.MotherTelephoneOffice,
                    [StudentExcelTemplate.MotherTelephoneResidence] = s.GuardianInfo?.MotherTelephoneResidence,

                    // Local Guardian
                    [StudentExcelTemplate.LocalGuardianName] = s.LocalGuardianInfo?.Name,
                    [StudentExcelTemplate.LocalGuardianRelation] = s.LocalGuardianInfo?.RelationToStudent,
                    [StudentExcelTemplate.LocalGuardianPhone] = s.LocalGuardianInfo?.Phone,
                    [StudentExcelTemplate.LocalGuardianEmail] = s.LocalGuardianInfo?.Email,
                    [StudentExcelTemplate.LocalGuardianAddress] = s.LocalGuardianInfo?.Address,
                };

                rows.Add(row);
            }

            var content = StudentExcelTemplateBuilder.BuildExport(classNames, rows);

            var fileName = request.ExportAll
                ? $"All_Students_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                : $"Students_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return Result.Success(new StudentExcelFileResponse
            {
                FileName = fileName,
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Content = content,
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentExcelFileResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
