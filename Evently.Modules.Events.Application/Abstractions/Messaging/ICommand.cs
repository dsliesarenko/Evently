using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.Abstractions.Messaging;

public interface ICommand : Mediator.ICommand<Result>;

public interface ICommand<TResponse> : Mediator.ICommand<Result<TResponse>>;
