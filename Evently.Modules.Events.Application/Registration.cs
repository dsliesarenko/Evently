using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Application;

public static class Registration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediator(config =>
        {
            config.Assemblies = [typeof(Application.AssemblyReference)];
            config.ServiceLifetime = ServiceLifetime.Scoped;
        });
    }
}
