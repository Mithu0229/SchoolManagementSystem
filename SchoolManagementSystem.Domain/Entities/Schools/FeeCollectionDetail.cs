namespace SchoolManagementSystem.Domain.Entities;

public class FeeCollectionDetail : AuditableEntity
{

    public Guid FeeCollectionId { get; set; }
    public Guid FeeHeadId { get; set; }

    public Guid MonthNo { get; set; }
    public Guid YearNo { get; set; }

    public decimal FeeAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }

    public FeeCollection FeeCollection { get; set; }
    public FeeHead FeeHead { get; set; }
}