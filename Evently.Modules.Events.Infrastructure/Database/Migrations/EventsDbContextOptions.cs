using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evently.Modules.Events.Infrastructure.Database.Migrations;

internal static class EventsDbContextOptions
{
    public static DbContextOptionsBuilder ConfigureEventsDbContext(
        this DbContextOptionsBuilder options,
        string connectionString
    )
    {
        return options
            .UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        Schemas.Events
                    );
                }
            )
            .UseSnakeCaseNamingConvention();
    }
}
