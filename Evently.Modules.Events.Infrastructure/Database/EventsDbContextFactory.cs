using Evently.Modules.Events.Infrastructure.Database.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContextFactory : IDesignTimeDbContextFactory<EventsDbContext>
{
    public EventsDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<EventsDbContext>();

        optionsBuilder.ConfigureEventsDbContext(connectionString);

        return new EventsDbContext(optionsBuilder.Options);
    }
}
