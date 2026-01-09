using MediatR;

namespace SchoolManagementSystem.Application.Common;

public interface IHttpRequestHandler<in TRequest> : IRequestHandler<TRequest, IResult>
where TRequest : IHttpRequest
{
}

public interface IHttpRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
}
