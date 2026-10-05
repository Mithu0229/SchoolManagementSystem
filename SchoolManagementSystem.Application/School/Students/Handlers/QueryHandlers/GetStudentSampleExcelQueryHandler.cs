using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Excel;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;

public class GetStudentSampleExcelQueryHandler : IHttpRequestHandler<GetStudentSampleExcelQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentSampleExcelQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentSampleExcelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Same source as the admission form's "Application For Class" dropdown.
            var classNames = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted(true, true)
                .Where(x => x.IsActive)
                .OrderBy(x => x.ClassName)
                .Select(x => x.ClassName)
                .ToListAsync(cancellationToken);

            var content = StudentExcelTemplateBuilder.Build(classNames);

            return Result.Success(new StudentExcelFileResponse
            {
                FileName = "Student_Upload_Sample.xlsx",
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
