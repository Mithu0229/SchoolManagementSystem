using SchoolManagementSystem.Application.School.Sections.Models;

namespace SchoolManagementSystem.Application.School.Sections.Commands;

public class InsertSectionCommand : IHttpRequest
{
    public SectionRequest Section { get; set; }
}
