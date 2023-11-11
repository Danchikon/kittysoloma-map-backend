using MediatR;

namespace KittysolomaMap.Application.Common.Mediator;


public abstract class CommandHandlerBase<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
{
    public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand> where TCommand : IRequest
{
    public abstract Task Handle(TCommand command, CancellationToken cancellationToken);
}