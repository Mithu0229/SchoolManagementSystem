using System;

namespace SchoolManagementSystem.Application.School.TrialBalances.Models;

public class TrialBalanceItemDto
{
    public string Particulars { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal DebitTrans { get; set; }
    public decimal CreditTrans { get; set; }
    public decimal NetTrans { get; set; }
    public decimal Dr { get; set; }
    public decimal Cr { get; set; }
}

public class TrialBalanceReportDto
{
    public List<TrialBalanceItemDto> Items { get; set; } = new List<TrialBalanceItemDto>();
}
