using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> : Mediator.ICommandHandler<TCommand, Result>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse>
    : Mediator.ICommandHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;
