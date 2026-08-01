using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Carts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddInfrastructure(configuration);
        //services.AddApplication();
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Can be used later"
    )]
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<CartService>();
    }
}
