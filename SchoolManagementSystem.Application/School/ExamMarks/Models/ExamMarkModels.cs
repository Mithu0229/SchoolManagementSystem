using System.Text.Json.Serialization;

namespace SchoolManagementSystem.Application.School.ExamMarks.Models;

public class ExamComponentDto
{
    public Guid Id { get; set; }
    public Guid? AcademicSessionId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public string ComponentName { get; set; } = string.Empty;
    public decimal MaxMarks { get; set; }
    public int OrderIndex { get; set; }
}

public class SingleMarkInput
{
    public Guid ExamComponentId { get; set; }
    public decimal MarksObtained { get; set; }
    public bool IsAbsent { get; set; }
}

public class SubjectMarkInputRow
{
    public Guid SubjectId { get; set; }
    public List<SingleMarkInput> ComponentMarks { get; set; } = new();
}

public class SaveStudentMarksRequest
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid StudentId { get; set; }
    public List<SubjectMarkInputRow> SubjectMarks { get; set; } = new();
}

// Model for batch mark entry (Class-wise for a single exam component)
public class BatchStudentMarkItem
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
    public decimal? MarksObtained { get; set; }
    public bool IsAbsent { get; set; }
}

public class SaveBatchMarksRequest
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ExamComponentId { get; set; }
    public List<BatchStudentMarkItem> StudentMarks { get; set; } = new();
}

// Marksheet Response formatted exactly matching Screenshot 1
public class MarksheetSubjectRowDto
{
    [JsonPropertyName("subjectId")]
    public Guid SubjectId { get; set; }

    [JsonPropertyName("subjectName")]
    public string SubjectName { get; set; } = string.Empty;

    // Term 1
    [JsonPropertyName("ct1")]
    public decimal? CT1 { get; set; }

    [JsonPropertyName("ct2")]
    public decimal? CT2 { get; set; }

    [JsonPropertyName("halfYearly")]
    public decimal? HalfYearly { get; set; }

    [JsonPropertyName("term1Total")]
    public decimal Term1Total { get; set; } // CT1 + CT2 + HalfYearly (Max: 100)

    // Term 2
    [JsonPropertyName("ct3")]
    public decimal? CT3 { get; set; }

    [JsonPropertyName("ct4")]
    public decimal? CT4 { get; set; }

    [JsonPropertyName("yearlyExam")]
    public decimal? YearlyExam { get; set; }

    [JsonPropertyName("term2Total")]
    public decimal Term2Total { get; set; } // CT3 + CT4 + YearlyExam (Max: 100)

    // Overall
    [JsonPropertyName("overallAverage")]
    public decimal OverallAverage { get; set; } // (Term1Total + Term2Total) / 2

    [JsonPropertyName("grade")]
    public string Grade { get; set; } = "F";

    [JsonPropertyName("gradePoint")]
    public decimal GradePoint { get; set; } = 0.00m;
}

public class MarksheetSummaryRowDto
{
    [JsonPropertyName("totalCT1")]
    public decimal TotalCT1 { get; set; }

    [JsonPropertyName("totalCT2")]
    public decimal TotalCT2 { get; set; }

    [JsonPropertyName("totalHalfYearly")]
    public decimal TotalHalfYearly { get; set; }

    [JsonPropertyName("totalTerm1")]
    public decimal TotalTerm1 { get; set; }

    [JsonPropertyName("totalCT3")]
    public decimal TotalCT3 { get; set; }

    [JsonPropertyName("totalCT4")]
    public decimal TotalCT4 { get; set; }

    [JsonPropertyName("totalYearlyExam")]
    public decimal TotalYearlyExam { get; set; }

    [JsonPropertyName("totalTerm2")]
    public decimal TotalTerm2 { get; set; }

    [JsonPropertyName("overallAverage")]
    public decimal OverallAverage { get; set; }

    [JsonPropertyName("finalGrade")]
    public string FinalGrade { get; set; } = "F";

    [JsonPropertyName("finalGPA")]
    public decimal FinalGPA { get; set; } = 0.00m;
}

public class GradingScaleLegendItem
{
    public string MarksRange { get; set; } = string.Empty;
    public string LetterGrade { get; set; } = string.Empty;
    public decimal GPA { get; set; }
}

public class StudentMarksheetResponse
{
    public string AcademicSession { get; set; } = string.Empty;
    public Guid AcademicSessionId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentRoll { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public Guid? SectionId { get; set; }

    public List<ExamComponentDto> Components { get; set; } = new();
    public List<MarksheetSubjectRowDto> Subjects { get; set; } = new();
    public MarksheetSummaryRowDto Summary { get; set; } = new();
    public List<GradingScaleLegendItem> GradingScaleLegend { get; set; } = new();
}
