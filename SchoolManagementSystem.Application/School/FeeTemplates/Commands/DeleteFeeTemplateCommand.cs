namespace SchoolManagementSystem.Application.School.FeeTemplates.Commands;

public record DeleteFeeTemplateCommand(Guid id) : IHttpRequest;
