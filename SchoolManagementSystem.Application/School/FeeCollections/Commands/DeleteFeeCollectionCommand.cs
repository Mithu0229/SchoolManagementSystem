namespace SchoolManagementSystem.Application.School.FeeCollections.Commands;

public record DeleteFeeCollectionCommand(Guid id) : IHttpRequest;
