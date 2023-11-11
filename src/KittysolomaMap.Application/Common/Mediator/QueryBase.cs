using MediatR;

namespace KittysolomaMap.Application.Common.Mediator;

public record QueryBase<TResponse> : IRequest<TResponse>;