namespace SchoolManagementSystem.Domain.Common;
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    Guid? DeletedById { get; set; }
    DateTime? DeletedDate { get; set; }
}