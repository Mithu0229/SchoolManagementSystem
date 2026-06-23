namespace SchoolManagementSystem.Domain.Entities;

public class BillMaster : AuditableEntity
{
    public Guid AdmissionId { get; set; }

    public int BillMonth { get; set; }
    public int BillYear { get; set; }

    public decimal TotalAmount { get; set; }

    public Admission Admission { get; set; }

    public ICollection<BillDetail> Details { get; set; }
}
