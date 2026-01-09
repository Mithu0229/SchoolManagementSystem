namespace SchoolManagementSystem.Domain.Entities;

public class CoreEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Guid? DeletedById { get; set; }
    public DateTime? DeletedDate { get; set; }
}
