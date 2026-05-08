using SchoolManagementSystem.Application.School.AcademicClasses.Models;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Commands;

public class InsertAcademicClassCommand : IHttpRequest
{
    public AcademicClassRequest AcademicClass { get; set; }
}
