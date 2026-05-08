using SchoolManagementSystem.Application.School.AcademicClasses.Models;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Commands;

public class UpdateAcademicClassCommand : IHttpRequest
{
    public AcademicClassRequest AcademicClass { get; set; }
}
