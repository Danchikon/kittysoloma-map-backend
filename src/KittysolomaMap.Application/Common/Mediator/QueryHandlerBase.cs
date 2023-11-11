using MediatR;

namespace KittysolomaMap.Application.Common.Mediator;

public abstract class QueryHandlerBase<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IRequest<TResponse>
{
    public abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}