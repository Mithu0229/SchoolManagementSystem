using System.Collections.Generic;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class GradingScale : AuditableEntity
{
    public required string ScaleName { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = true;

    public ICollection<GradingScaleDetail> Details { get; set; } = new List<GradingScaleDetail>();
}
