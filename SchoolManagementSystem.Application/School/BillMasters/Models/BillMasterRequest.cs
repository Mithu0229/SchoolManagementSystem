namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class BillMasterRequest
{
    public Guid Id { get; set; }
    public string? StdCID { get; set; }
    public Guid AdmissionId { get; set; }
    public int BillMonth { get; set; }
    public int BillYear { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }
    public IList<BillDetailRequest> Details { get; set; } = new List<BillDetailRequest>();
}

public class BillDetailRequest
{
    public Guid Id { get; set; }
    public Guid FeeTemplateDetailId { get; set; }
    public Guid FeeHeadId { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>
/// Simplified request for bill process — only needs AdmissionId, BillMonth, and BillYear.
/// Details are auto-generated from the matching FeeTemplate.
/// </summary>
public class ProcessBillRequest
{
    public Guid AdmissionId { get; set; }
    public int BillMonth { get; set; }
    public int BillYear { get; set; }
}
