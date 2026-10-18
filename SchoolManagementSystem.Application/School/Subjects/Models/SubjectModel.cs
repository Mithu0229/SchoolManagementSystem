namespace SchoolManagementSystem.Application.School.Subjects.Models;

public class SubjectRequest
{
    public Guid? Id { get; set; }
    public required string SubjectName { get; set; }
    public string? SubjectCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SubjectResponse
{
    public Guid Id { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string? SubjectCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class ClassSubjectDto
{
    public Guid ClassId { get; set; }
    public List<Guid> SubjectIds { get; set; } = new();
}

public class AssignedSubjectDto
{
    public Guid SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string? SubjectCode { get; set; }
    public int OrderIndex { get; set; }
}
