namespace SchoolManagementSystem.Application.School.AdmitCards.Models;

public class StudentAdmitCardDto
{
    public string ExamType { get; set; } = string.Empty; // "CT", "HALF_YEARLY", "ANNUAL"
    public string ExamName { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public string ExamStartDate { get; set; } = string.Empty;
    public int CutoffMonth { get; set; }
    public int CutoffYear { get; set; }
    public decimal DueAmount { get; set; }
    public decimal DueAmountCurrentDate { get; set; }
    public decimal DueAmountExamDate { get; set; }
    public bool IsEligible { get; set; }
    public string Status { get; set; } = string.Empty;
    public string BadgeColor { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public class StudentAdmitCardListResponse
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public decimal TotalUnpaidDueOverall { get; set; }
    public List<StudentAdmitCardDto> AdmitCards { get; set; } = new();
}

public record AdmitCardPdfResponse(byte[] Content, string ContentType, string FileName);
