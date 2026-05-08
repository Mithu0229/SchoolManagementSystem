using SchoolManagementSystem.Application.School.Shifts.Models;

namespace SchoolManagementSystem.Application.School.Shifts.Commands;

public class UpdateShiftCommand : IHttpRequest
{
    public ShiftRequest Shift { get; set; }
}
