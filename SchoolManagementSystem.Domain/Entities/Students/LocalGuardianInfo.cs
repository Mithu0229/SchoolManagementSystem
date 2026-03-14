namespace SchoolManagementSystem.Domain.Entities.Students;

public class LocalGuardianInfo :CoreEntity
{
    public  string? Name { get; set; }
    public  string? RelationToStudent { get; set; }
    public string? Address { get; set; }
    public  string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid StudentInfoId { get; set; }
    public virtual StudentInfo? StudentInfo { get; set; }
}
