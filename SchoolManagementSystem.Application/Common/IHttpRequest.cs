using MediatR;

namespace SchoolManagementSystem.Application.Common;

public interface IHttpRequest : IRequest<IResult>
{
}

public interface IHttpRequest<out TResponse> : IRequest<TResponse>
{
}
