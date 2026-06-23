namespace SchoolManagementSystem.Domain.Entities;

public class BillDetail : AuditableEntity
{
    public Guid BillMasterId { get; set; }
    public Guid? FeeTemplateDetailId { get; set; }
    public Guid? FeeHeadId { get; set; }

    public decimal Amount { get; set; }

    public BillMaster BillMaster { get; set; }
    public FeeTemplateDetail FeeTemplateDetail { get; set; }
    public FeeHead FeeHead { get; set; }
}
