namespace SchoolManagementSystem.Application.School.Students.Commands;

/// <summary>Imports students from an uploaded Excel file using the standard insert flow.</summary>
public class ImportStudentExcelCommand : IHttpRequest
{
    public IFormFile? File { get; set; }
}
