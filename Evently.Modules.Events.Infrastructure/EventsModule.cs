using Evently.Modules.Events.Application;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Data;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Database.Migrations;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddApplication();

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(
            databaseConnectionString
        ).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddDbContext<EventsDbContext>(options =>
        {
            options.ConfigureEventsDbContext(databaseConnectionString);
        });

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
    }
}
