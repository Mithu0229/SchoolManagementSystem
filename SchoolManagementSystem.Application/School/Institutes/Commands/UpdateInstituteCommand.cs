using SchoolManagementSystem.Application.School.Institutes.Models;

namespace SchoolManagementSystem.Application.School.Institutes.Commands;

public class UpdateInstituteCommand : IHttpRequest
{
    public InstituteRequest Institute { get; set; }
}
