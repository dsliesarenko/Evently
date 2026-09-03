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

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak(
        "keycloak",
        8080,
        builder.CreateResourceBuilder(new ParameterResource("keycloak-user", _ => "admin")),
        builder.CreateResourceBuilder(new ParameterResource("keycloak-password", _ => "admin"))
    )
    .WithDataVolume("keycloak-data-volume")
    .WithLifetime(ContainerLifetime.Persistent);

builder
    .AddProject<Projects.Evently_Api>("evently-api")
    .WithEnvironment("ConnectionStrings__Database", db)
    .WithReference(cache)
    .WithReference(keycloak)
    .WaitFor(db)
    .WaitFor(cache)
    .WaitFor(keycloak);

await builder.Build().RunAsync();
