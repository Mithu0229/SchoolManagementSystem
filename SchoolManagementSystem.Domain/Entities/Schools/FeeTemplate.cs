namespace SchoolManagementSystem.Domain.Entities;

public class FeeTemplate : AuditableEntity
{
    public string TemplateName { get; set; }

    public Guid ClassId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? ShiftId { get; set; }

    public AcademicClass Class { get; set; }
    public StudentGroup? Group { get; set; }
    public Shift? Shift { get; set; }

    public ICollection<FeeTemplateDetail> Details { get; set; }
}