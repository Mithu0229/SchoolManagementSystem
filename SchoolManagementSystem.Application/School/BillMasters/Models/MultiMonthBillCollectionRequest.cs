using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class MultiMonthBillCollectionRequest
{
    public Guid StudentId { get; set; }
    public List<Guid> BillMasterIds { get; set; } = new List<Guid>();
    public decimal CollectionAmount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? BankName { get; set; }
    public string? TrxId { get; set; }
    public string? AccountNo { get; set; }
    public string? TransactionNo { get; set; }
    public string? VoucherNo { get; set; }
    public string? Particulars { get; set; }
    public int? BillYear { get; set; }
    public int? BillMonth { get; set; }
    public List<BillDetailRequest> AdditionalDetails { get; set; } = new List<BillDetailRequest>();
}
