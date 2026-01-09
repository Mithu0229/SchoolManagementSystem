namespace SchoolManagementSystem.Application.GS.Users.Queries;
public record GetUserByIdQuery(Guid Id) : IHttpRequest;
