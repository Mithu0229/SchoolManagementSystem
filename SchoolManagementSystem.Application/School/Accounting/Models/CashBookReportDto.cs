using System;

namespace SchoolManagementSystem.Application.School.Accounting.Models;

public class CashBookReportDto
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Particulars { get; set; }
    public string VoucherNo { get; set; }
    public string AccountNo { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
}
