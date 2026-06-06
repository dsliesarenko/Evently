using Evently.Common.Domain;

namespace Evently.Common.Application.Messaging;

public interface ICommand : Mediator.ICommand<Result>;

public interface ICommand<TResponse> : Mediator.ICommand<Result<TResponse>>;
