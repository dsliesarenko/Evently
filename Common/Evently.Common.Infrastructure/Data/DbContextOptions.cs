using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evently.Common.Infrastructure.Data;

public static class DbContextOptions
{
    public static DbContextOptionsBuilder ConfigureDbContext(
        this DbContextOptionsBuilder options,
        string connectionString,
        string schemaName
    )
    {
        return options
            .UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        schemaName
                    );
                }
            )
            .UseSnakeCaseNamingConvention();
    }
}
