namespace SchoolManagementSystem.Application.School.Students.Queries;

/// <summary>Generates the Student list Excel export populated with filtered or all students.</summary>
public class ExportStudentListExcelQuery : IHttpRequest
{
    public string? Search { get; set; }
    public string? ClassId { get; set; }
    public List<Guid>? StudentIds { get; set; }
    public bool ExportAll { get; set; }
}
