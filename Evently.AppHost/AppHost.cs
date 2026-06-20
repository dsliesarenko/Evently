IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(
        "pgsql",
        builder.CreateResourceBuilder(new ParameterResource("postgres-user", _ => "postgres")),
        builder.CreateResourceBuilder(new ParameterResource("postgres-password", _ => "postgres")),
        5432
    )
    .WithVolume("postgresql-data-volume-v18", "/var/lib/postgresql")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<PostgresDatabaseResource> db = postgres.AddDatabase("evently");

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache", port: 6379)
    .WithLifetime(ContainerLifetime.Persistent);

builder
    .AddProject<Projects.Evently_Api>("evently-api")
    .WithEnvironment("ConnectionStrings__Database", db)
    .WithReference(cache)
    .WaitFor(db)
    .WaitFor(cache);

await builder.Build().RunAsync();
