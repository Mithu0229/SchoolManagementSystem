namespace SchoolManagementSystem.Application.School.FeeTemplates.Queries;

public record GetFeeTemplateByIdQuery(Guid Id) : IHttpRequest;
