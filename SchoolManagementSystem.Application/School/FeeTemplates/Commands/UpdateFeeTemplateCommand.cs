using SchoolManagementSystem.Application.School.FeeTemplates.Models;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Commands;

public class UpdateFeeTemplateCommand : IHttpRequest
{
    public FeeTemplateRequest FeeTemplate { get; set; }
}
