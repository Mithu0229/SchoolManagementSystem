namespace SchoolManagementSystem.Application.School.FeeHeads.Queries;

public record GetFeeHeadByIdQuery(Guid Id) : IHttpRequest;
