using Evently.Common.Application.Behaviors;

namespace Evently.Api.Extensions;

internal static class MediatorExtensions
{
    public static IServiceCollection AddApplicationMediator(this IServiceCollection services)
    {
        services.AddMediator(config =>
        {
            config.Assemblies =
            [
                typeof(Modules.Events.Application.AssemblyReference),
                typeof(Modules.Users.Application.AssemblyReference),
                typeof(Modules.Users.Domain.AssemblyReference), //Add all handlers and remove this line
                typeof(Modules.Ticketing.Application.AssemblyReference),
            ];

            config.ServiceLifetime = ServiceLifetime.Scoped;

            config.PipelineBehaviors =
            [
                typeof(ExceptionHandlingPipelineBehavior<,>),
                typeof(RequestLoggingPipelineBehavior<,>),
                typeof(ValidationPipelineBehavior<,>),
            ];
        });

        return services;
    }
}
