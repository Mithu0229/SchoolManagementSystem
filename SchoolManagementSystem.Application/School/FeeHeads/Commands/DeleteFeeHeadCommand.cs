namespace SchoolManagementSystem.Application.School.FeeHeads.Commands;

public record DeleteFeeHeadCommand(Guid id) : IHttpRequest;
