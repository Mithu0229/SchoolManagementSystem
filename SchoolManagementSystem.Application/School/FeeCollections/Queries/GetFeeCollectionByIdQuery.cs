namespace SchoolManagementSystem.Application.School.FeeCollections.Queries;

public record GetFeeCollectionByIdQuery(Guid Id) : IHttpRequest;
