using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.Application.School.Accounting.Models;

public class BalanceSheetItemDto
{
    public string Particulars { get; set; }
    public decimal Amount { get; set; }
}

public class BalanceSheetDto
{
    public DateTime AsOfDate { get; set; }

    public List<BalanceSheetItemDto> Assets { get; set; } = new List<BalanceSheetItemDto>();
    public List<BalanceSheetItemDto> LiabilitiesAndEquity { get; set; } = new List<BalanceSheetItemDto>();

    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilitiesAndEquity { get; set; }
}
