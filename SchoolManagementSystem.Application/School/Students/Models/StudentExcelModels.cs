namespace SchoolManagementSystem.Application.School.Students.Models;

/// <summary>Multipart request for the student Excel upload.</summary>
public class StudentExcelUploadRequest
{
    public IFormFile? File { get; set; }
}

/// <summary>Generated file returned by the sample download.</summary>
public class StudentExcelFileResponse
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}

/// <summary>Summary returned after processing an uploaded student Excel.</summary>
public class StudentExcelImportResponse
{
    public int TotalRows { get; set; }
    public int ImportedCount { get; set; }
    public int FailedCount { get; set; }

    /// <summary>True when the file was rejected during validation and nothing was saved.</summary>
    public bool ValidationFailed { get; set; }

    public List<StudentExcelRowError> Errors { get; set; } = new();
    public List<StudentExcelImportedRow> Imported { get; set; } = new();
}

public class StudentExcelRowError
{
    public int RowNumber { get; set; }
    public string? FullName { get; set; }
    public List<string> Messages { get; set; } = new();
}

public class StudentExcelImportedRow
{
    public int RowNumber { get; set; }
    public Guid StudentId { get; set; }
    public string? StdCID { get; set; }
    public string? FullName { get; set; }
}
