namespace SchoolManagementSystem.Domain.Entities;

public class Institute : AuditableEntity
{
    public required string InstituteName { get; set; }
    public string? Address { get; set; }
    public string? ContactNo { get; set; }
    public string? Email { get; set; }
    public string? LogoPath { get; set; }

    public ICollection<Branch> Branches { get; set; }
}
