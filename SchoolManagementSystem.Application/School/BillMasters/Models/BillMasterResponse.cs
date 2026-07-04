namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class BillMasterResponse
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public string? AdmissionRollNo { get; set; }
    public string? StdCID { get; set; }
    public int BillMonth { get; set; }
    public int BillYear { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }
    public IList<BillDetailResponse> Details { get; set; } = new List<BillDetailResponse>();
}

public class BillDetailResponse
{
    public Guid Id { get; set; }
    public Guid BillMasterId { get; set; }
    public Guid? FeeTemplateDetailId { get; set; }
    public Guid? FeeHeadId { get; set; }
    public string? FeeHeadName { get; set; }
    public decimal Amount { get; set; }
}
