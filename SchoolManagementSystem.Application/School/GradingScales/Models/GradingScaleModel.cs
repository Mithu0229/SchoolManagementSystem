namespace SchoolManagementSystem.Application.School.GradingScales.Models;

public class GradingScaleDetailDto
{
    public Guid? Id { get; set; }
    public decimal MinMark { get; set; }
    public decimal MaxMark { get; set; }
    public required string LetterGrade { get; set; }
    public decimal GradePoint { get; set; }
    public string? Remarks { get; set; }
    public int OrderIndex { get; set; }
}

public class GradingScaleRequest
{
    public Guid? Id { get; set; }
    public required string ScaleName { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = true;
    public List<GradingScaleDetailDto> Details { get; set; } = new();
}

public class GradingScaleResponse
{
    public Guid Id { get; set; }
    public string ScaleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public List<GradingScaleDetailDto> Details { get; set; } = new();
}
