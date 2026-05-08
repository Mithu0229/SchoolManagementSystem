namespace SchoolManagementSystem.Domain.Entities;

public class FeeHead : AuditableEntity
{
    public required string FeeHeadName { get; set; }
    public bool IsMonthly { get; set; }
}