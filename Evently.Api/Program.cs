using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using Microsoft.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                },
            };

            foreach (
                KeyValuePair<
                    HttpMethod,
                    OpenApiOperation
                > operation in document.Paths.Values.SelectMany(path => path.Operations)
            )
            {
                operation.Value.Security ??= [];

                operation.Value.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
                    }
                );
            }

            return Task.CompletedTask;
        }
    );
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationCommon([
    Evently.Modules.Events.Presentation.AssemblyReference.Assembly,
    Evently.Modules.Users.Presentation.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Presentation.AssemblyReference.Assembly,
]);

string dbConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;
string keycloakUrl = builder.Configuration["Keycloak:Url"]!;

builder.Services.AddInfrastructure(
    [TicketingModule.ConfigureConsumers],
    dbConnectionString,
    redisConnectionString
);

builder.Configuration.AddModuleConfiguration(["events", "users", "ticketing"]);

builder
    .Services.AddHealthChecks()
    .AddNpgSql(dbConnectionString)
    .AddRedis(redisConnectionString)
    .AddUrlGroup(new Uri(keycloakUrl), HttpMethod.Get, "keycloak");

builder.Services.AddApplicationMediator();

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

    app.ApplyMigrations();
}

app.MapEndpoints();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
