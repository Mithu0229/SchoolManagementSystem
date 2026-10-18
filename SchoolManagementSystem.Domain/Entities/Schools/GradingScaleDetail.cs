namespace SchoolManagementSystem.Domain.Entities.Schools;

public class GradingScaleDetail : CoreEntity
{
    public Guid GradingScaleId { get; set; }
    public decimal MinMark { get; set; }
    public decimal MaxMark { get; set; }
    public required string LetterGrade { get; set; }
    public decimal GradePoint { get; set; }
    public string? Remarks { get; set; }
    public int OrderIndex { get; set; } = 0;

    public GradingScale? GradingScale { get; set; }
}
