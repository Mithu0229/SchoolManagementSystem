namespace SchoolManagementSystem.Application.School.Shifts.Models;

public class ShiftRequest
{
    public Guid Id { get; set; }
    public string ShiftName { get; set; }
    public bool IsActive { get; set; }
}
