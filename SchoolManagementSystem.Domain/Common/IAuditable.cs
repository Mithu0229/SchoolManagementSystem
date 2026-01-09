namespace SchoolManagementSystem.Domain.Common;
public interface IAuditable
{
    bool IsActive { get; set; }
    Guid CreatedById { get; set; }
    DateTime CreatedDate { get; set; }
    Guid? ModifiedById { get; set; }
    DateTime? ModifiedDate { get; set; }
}

