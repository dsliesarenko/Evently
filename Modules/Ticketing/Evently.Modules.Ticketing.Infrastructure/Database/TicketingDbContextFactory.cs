using Evently.Common.Infrastructure.Data;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Evently.Modules.Users.Infrastructure.Database;

public sealed class TicketingDbContextFactory : IDesignTimeDbContextFactory<TicketingDbContext>
{
    public TicketingDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();

        optionsBuilder.ConfigureDbContext(connectionString, Schemas.Ticketing);

        return new TicketingDbContext(optionsBuilder.Options);
    }
}
