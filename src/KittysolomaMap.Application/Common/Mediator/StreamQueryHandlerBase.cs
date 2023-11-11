using MediatR;

namespace KittysolomaMap.Application.Common.Mediator;

public abstract class StreamQueryHandlerBase<TStreamQuery, TResponse> : IStreamRequestHandler<TStreamQuery, TResponse>
    where TStreamQuery : IStreamRequest<TResponse>
{
    public abstract IAsyncEnumerable<TResponse> Handle(TStreamQuery query, CancellationToken cancellationToken);
}