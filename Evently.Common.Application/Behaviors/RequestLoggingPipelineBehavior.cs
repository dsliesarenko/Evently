using System.Diagnostics;
using Evently.Common.Domain;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Evently.Common.Application.Behaviors;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IMessage
    where TResponse : Result
{
    public async ValueTask<TResponse> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        Type requestType = typeof(TRequest);

        string moduleName = GetModuleName(requestType);
        string requestName = requestType.Name;

        using IDisposable? scope = logger.BeginScope(
            new Dictionary<string, object?>
            {
                ["Module"] = moduleName,
                ["RequestName"] = requestName,
                ["RequestType"] = requestType.FullName,
            }
        );

        Activity.Current?.SetTag("evently.module", moduleName);
        Activity.Current?.SetTag("evently.request.name", requestName);
        Activity.Current?.SetTag("evently.request.type", requestType.FullName);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Processing request {RequestName}", requestName);
        }

        try
        {
            TResponse result = await next(message, cancellationToken);

            if (result.IsSuccess)
            {
                Activity.Current?.SetTag("evently.request.success", true);

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Completed request {RequestName}", requestName);
                }
            }
            else
            {
                Activity.Current?.SetTag("evently.request.success", false);
                Activity.Current?.SetTag("evently.error.code", result.Error.Code);
                Activity.Current?.SetTag("evently.error.description", result.Error.Description);

                using IDisposable? errorScope = logger.BeginScope(
                    new Dictionary<string, object?>
                    {
                        ["ErrorCode"] = result.Error.Code,
                        ["ErrorDescription"] = result.Error.Description,
                    }
                );

                logger.LogWarning(
                    "Completed request {RequestName} with error {ErrorCode}: {ErrorDescription}",
                    requestName,
                    result.Error.Code,
                    result.Error.Description
                );
            }

            return result;
        }
        catch (Exception exception)
        {
            Activity.Current?.SetTag("evently.request.success", false);
            Activity.Current?.SetStatus(ActivityStatusCode.Error, exception.Message);

            throw;
        }
    }

    private static string GetModuleName(Type requestType)
    {
        string? fullName = requestType.FullName;

        if (string.IsNullOrWhiteSpace(fullName))
        {
            return "Unknown";
        }

        string[] parts = fullName.Split('.');

        return parts.Length > 2 ? parts[2] : "Unknown";
    }
}
