using Evently.Common.Application.Exceptions;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Evently.Common.Application.Behaviors;

public sealed class ExceptionHandlingPipelineBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IMessage
{
    public async ValueTask<TResponse> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next(message, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Unhandled exception for {RequestName}",
                typeof(TRequest).Name
            );

            throw new EventlyException(typeof(TRequest).Name, innerException: exception);
        }
    }
}
