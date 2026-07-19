using Evently.Common.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Evently.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

        optionsBuilder.ConfigureDbContext(connectionString, Schemas.Users);

        return new UsersDbContext(optionsBuilder.Options);
    }
}
