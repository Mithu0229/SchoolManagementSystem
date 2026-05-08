namespace SchoolManagementSystem.Domain.Entities;

public class FeeTemplateDetail : AuditableEntity
{

    public Guid FeeTemplateId { get; set; }
    public Guid FeeHeadId { get; set; }

    public decimal Amount { get; set; }

    public FeeTemplate FeeTemplate { get; set; }
    public FeeHead FeeHead { get; set; }
}