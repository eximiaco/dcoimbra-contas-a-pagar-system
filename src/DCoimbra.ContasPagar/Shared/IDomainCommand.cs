using CSharpFunctionalExtensions;
using MediatR;

namespace DCoimbra.Shared;

public interface IDomainCommand
{
    Guid Id { get; }
    DateTime Timestamp { get; }
}

public interface IDomainCommand<out TRequest> : IDomainCommand, IRequest<TRequest>
{
}