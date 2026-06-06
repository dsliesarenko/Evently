IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> db = builder
    .AddPostgres(
        "pgsql",
        builder.CreateResourceBuilder(new ParameterResource("postgres-user", _ => "postgres")),
        builder.CreateResourceBuilder(new ParameterResource("postgres-password", _ => "postgres")),
        5432
    )
    .WithVolume("postgresql-data-volume", "/var/lib/postgresql/data")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("evently");

builder
    .AddProject<Projects.Evently_Api>("evently-api")
    .WithEnvironment("ConnectionStrings__Database", db)
    .WaitFor(db);

await builder.Build().RunAsync();
