using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.Application.School.Accounting.Models;

public class IncomeStatementItemDto
{
    public string Particulars { get; set; }
    public decimal Amount { get; set; }
}

public class IncomeStatementDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    public List<IncomeStatementItemDto> Revenues { get; set; } = new List<IncomeStatementItemDto>();
    public List<IncomeStatementItemDto> Expenses { get; set; } = new List<IncomeStatementItemDto>();

    public decimal TotalRevenue { get; set; }
    public decimal TotalExpense { get; set; }
    
    public decimal NetIncome => TotalRevenue - TotalExpense;
}
