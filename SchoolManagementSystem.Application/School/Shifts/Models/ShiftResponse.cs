namespace SchoolManagementSystem.Application.School.Shifts.Models;

public class ShiftResponse
{
    public Guid Id { get; set; }
    public string ShiftName { get; set; }
    public bool IsActive { get; set; }
}
