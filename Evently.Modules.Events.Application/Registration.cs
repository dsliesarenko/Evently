using Evently.Common.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Application;

public static class Registration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediator(config =>
        {
            config.Assemblies = [typeof(AssemblyReference)];
            config.ServiceLifetime = ServiceLifetime.Scoped;

            config.PipelineBehaviors = [typeof(RequestLoggingPipelineBehavior<,>)];
        });
    }
}
