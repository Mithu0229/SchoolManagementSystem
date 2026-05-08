using SchoolManagementSystem.Application.School.AcademicSessions.Models;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Commands;

public class InsertAcademicSessionCommand : IHttpRequest
{
    public AcademicSessionRequest AcademicSession { get; set; }
}
