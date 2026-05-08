namespace SchoolManagementSystem.Application.School.Branches.Queries;

public record GetBranchByIdQuery(Guid Id) : IHttpRequest;
