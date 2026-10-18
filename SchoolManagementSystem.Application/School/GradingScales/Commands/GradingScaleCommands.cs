using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.GradingScales.Models;

namespace SchoolManagementSystem.Application.School.GradingScales.Commands;

public class SaveGradingScaleCommand : IHttpRequest
{
    public required GradingScaleRequest GradingScale { get; set; }
}

public class DeleteGradingScaleCommand(Guid id) : IHttpRequest
{
    public Guid Id { get; set; } = id;
}
