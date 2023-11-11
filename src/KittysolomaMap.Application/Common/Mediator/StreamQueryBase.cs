using MediatR;

namespace KittysolomaMap.Application.Common.Mediator;

public record StreamQueryBase<TResponse> : IStreamRequest<TResponse>;