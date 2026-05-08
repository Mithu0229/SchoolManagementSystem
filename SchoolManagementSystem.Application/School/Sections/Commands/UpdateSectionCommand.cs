using SchoolManagementSystem.Application.School.Sections.Models;

namespace SchoolManagementSystem.Application.School.Sections.Commands;

public class UpdateSectionCommand : IHttpRequest
{
    public SectionRequest Section { get; set; }
}
